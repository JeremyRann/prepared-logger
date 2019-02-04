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
