/* This is a temporary space to keep some notes. I asked a Stack Overflow question here:
https://stackoverflow.com/q/54497275/2850538

I want OpenID Connect for authentication and sliding expiration where the user
will never be taken off of the page in normal circumstances; in other words,
I'm not willing to redirect on every access token expiration (which seems to be
the norm). It appears I have three options:

1. Don't use refresh tokens; issue short access tokens and use an iframe hack
to silently re-request access tokens. This is an old method, but it appeals to
me because under the hood it leverages http-only cookies, which I think we
should be using. IDServer4 uses .net's built-in cookie auth, and there's great
documentation on that here:
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2
Generally though, this approach is described here:
https://brockallen.com/2019/01/03/the-state-of-the-implicit-flow-in-oauth2/.
There are probably other resources that go into more detail about how to do
this.
2. Use refresh tokens by calling ID server directly from the SPA. It's
explicitly recommended against
(https://tools.ietf.org/html/draft-parecki-oauth-browser-based-apps-02#section-8:
Authorization servers SHOULD NOT issue refresh tokens to browser-based
applications. So obviously I don't like this option, however there are ways to
mitigate the danger. Ultimately, I feel XSS can never be ruled out with this
option; you'd need an http-only cookie to ensure all the data you'd need to
refresh isn't entirely in local storage, and I don't think there's really a way
to do that since it's a CORS request. I had originally envisioned refreshing
via the backend, but this appears to either not be possible or be
sufficiently non-standard that I'd be unwilling to do it.
3. Follow advice at
https://leastprivilege.com/2019/01/18/an-alternative-way-to-secure-spas-with-asp-net-core-openid-connect-oauth-2-0-and-proxykit/,
which I think basically involves using OpenID connect for initial authorization
then delegating the rest of the process to the backend. I find it to be a
cop-out in some ways, but if I were designing my own SSO flow, it'd probably
look like this. It's still off the beaten path though, so again I'm not
super-comfortable with it yet.

Note that whatever approach is taken, I'll want to check a few things:
1. I should be using a CSP as strict as possible
2. I should ensure same-site cookies are used
3. Obivously, all cookies should be http-only
4. Probably I should be using PKCE; I hate it since I don't think it solves any
problems I really have, but IETF appears to recommend it
(https://tools.ietf.org/html/draft-parecki-oauth-browser-based-apps-02#section-4:
For authorizing users within a browser-based application, the best
current practice is to...Use the OAuth 2.0 authorization code flow with
the PKCE extension. I'm not so confident in my understanding that I'm
willing to drop that.
5. External providers should still work (not sure if that's possible for all
options).
*/
import oidc from "oidc-client";
import Url from "url-parse";
import axios from "axios";

let baseUrl = new Url(window.location.href).set("pathname", "").set("query", "").set("hash", "");
let redirectUrl = (new Url(baseUrl)).set("query", "redirect=fromIdentityProvider");
let silentRedirectUrl = (new Url(baseUrl)).set("pathname", "silent-refresh.html");

let identitySettings = {
    authority: "http://localhost:5001",
    client_id: "js",
    redirect_uri: redirectUrl.toString(),
    response_type: "code",
    scope: "openid profile PreparedLogger",
    post_logout_redirect_uri: baseUrl,
    automaticSilentRenew: true,
    silent_redirect_uri: silentRedirectUrl.toString()
};

const useOidc = true;

let moduleExport;

if (useOidc) {
    let userManager = new oidc.UserManager(identitySettings);
    let user = null;

    moduleExport = {
        async logIn () {
            await userManager.signinRedirect();
        },
        async isLoggedIn () {
            return !!(await this.getAccessToken());
        },
        isGuest () {
            return user === null;
        },
        async logOut () {
            await userManager.signoutRedirect();
        },
        async getAccessToken () {
            user = await userManager.getUser();
            return user ? user.access_token : null;
        },
        async initializeApp () {
            // Hack to get access from a console while developing
            console.log("userManager", userManager);
            let url = new Url(window.location.href, true);

            if (url.query && url.query.redirect === "fromIdentityProvider") {
                await new oidc.UserManager({ response_mode: "query" }).signinRedirectCallback();
                window.location = "/";
                return false;
            }
            // user will be null if we don't already have a user in local storage, otherwise there will be an access token
            user = await userManager.getUser();
            return true;
        }
    };
}
else {
    let authData = null;

    moduleExport = {
        async logIn () {
            let stateArr = [];
            // Add 64 hex characters together for a random 32-byte hex string
            for (let i = 0; i < 64; i++) { stateArr.push(Math.trunc(Math.random() * 16).toString(16)); }
            let state = stateArr.join("");

            window.location.href = identitySettings["authority"] + "/connect/authorize?" + makeParams({
                "client_id": "js",
                "redirect_uri": identitySettings["redirect_uri"],
                "response_type": identitySettings["response_type"],
                "scope": identitySettings["scope"],
                "state": state
            });
        },
        async isLoggedIn () {
            return !!authData;
        },
        isGuest () {
            return authData === null;
        },
        async logOut () {
            delete sessionStorage["auth_data"];

            window.location.href = identitySettings["authority"] + "/connect/endsession?" + makeParams({
                "post_logout_redirect_uri": identitySettings["post_logout_redirect_uri"],
                "id_token_hint": authData.id_token
            });

            authData = null;
        },
        async getAccessToken () {
            return new Promise(async resolve => {
                if (authData === null) {
                    await this.logIn();
                }
                else {
                    resolve(authData.access_token);
                }
            });
        },
        async initializeApp () {
            let url = new Url(window.location.href, true);

            if (url.query && url.query.redirect === "fromIdentityProvider") {
                let request = axios.create({ baseURL: identitySettings.authority });
                let authResult = await request.post("/connect/token", makeParams({
                    "client_id": "js",
                    "code": url.query["code"],
                    "redirect_uri": identitySettings["redirect_uri"],
                    "grant_type": "authorization_code"
                }));
                sessionStorage["auth_data"] = JSON.stringify(authResult.data);
                window.location = "/";
                return false;
            }
            authData = (sessionStorage["auth_data"]) ? JSON.parse(sessionStorage["auth_data"]) : null;
            return true;
        }
    };
}

export default moduleExport;

function makeParams (obj) {
    return Object.keys(obj).map(key => encodeURIComponent(key) + "=" + encodeURIComponent(obj[key])).join("&");
}
