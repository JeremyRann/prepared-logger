import oidc from "oidc-client";
import Url from "url-parse";
import axios from "axios";

let identitySettings = {
    authority: "http://localhost:5001",
    client_id: "js",
    redirect_uri: "http://localhost:8080?redirect=fromIdentityProvider",
    response_type: "code",
    scope: "openid profile PreparedLogger",
    post_logout_redirect_uri: "http://localhost:8080"
};
let userManager = new oidc.UserManager(identitySettings);
let user = null;
let accessToken = null;

const useOidc = true;

// let identityProviderConfigPromise = null;
// async function getIdentityProviderConfig () {
//     if (identityProviderConfigPromise === null) {
//         identityProviderConfigPromise = new Promise(async resolve => {
//             let request = axios.create({ baseURL: identitySettings.authority });
//             resolve((await request.get("/.well-known/openid-configuration")).data);
//         });
//     }
//
//     return identityProviderConfigPromise;
// };

function makeParams (obj) {
    return Object.keys(obj).map(key => encodeURIComponent(key) + "=" + encodeURIComponent(obj[key])).join("&");
};

export default {
    async logIn () {
        if (useOidc) {
            await userManager.signinRedirect();
            return;
        }

        // let idConfig = await getIdentityProviderConfig();
        let stateArr = [];
        // Add 64 hex characters together for a random 32-byte hex string
        for (let i = 0; i < 64; i++) { stateArr.push(Math.trunc(Math.random() * 16).toString(16)); }
        let state = stateArr.join("");
        console.log(state);

        window.location.href = identitySettings["authority"] + "/connect/authorize?" + makeParams({
            "client_id": "js",
            "redirect_uri": identitySettings["redirect_uri"],
            "response_type": identitySettings["response_type"],
            "scope": identitySettings["scope"],
            "state": state
        });
    },
    async isLoggedIn () {
        if (useOidc) {
            let user = await userManager.getUser();
            console.log(["user", user]);
            return !!user;
        }
        return !!accessToken;
    },
    isGuest () {
        if (useOidc) {
            return user === null;
        }
        return accessToken === null;
    },
    async logOut () {
        await userManager.signoutRedirect();
    },
    async getAccessToken () {
        if (useOidc) {
            return new Promise(resolve => {
                if (user === null) {
                    userManager.signinRedirect();
                }
                else {
                    resolve(user.access_token);
                }
            });
        }
        else {
            return new Promise(async resolve => {
                if (accessToken === null) {
                    await this.logIn();
                }
                else {
                    resolve(accessToken);
                }
            });
        }
    },
    async initializeApp () {
        console.log(Url);
        // Go back to the app if this is an ID provider redirect
        let url = new Url(window.location.href, true);
        console.log(JSON.stringify(url, null, 4));

        if (useOidc) {
            if (url.query && url.query.redirect === "fromIdentityProvider") {
                await new oidc.UserManager({ response_mode: "query" }).signinRedirectCallback();
                window.location = "/";
                return false;
            }
            // user will be null if we don't already have a user in local storage, otherwise there will be an access token
            user = await userManager.getUser();
            return true;
        }

        // http://localhost:5001/connect/token
        // {
        //     "client_id": "js",
        //     "code": "9effbe8ebf62b0982fcd3b454c65d9c3efa7c1df4ee994a5aae93fd60dca27b2",
        //     "redirect_uri": "http://localhost:8080?redirect=fromIdentityProvider",
        //     "code_verifier": "aee9dc9ce0c343538e40778d458dd6363cb202313713445d9ecf3a7f15780a7ede6304cfb3f24fa5800a1a7baabec66a",
        //     "grant_type": "authorization_code"
        // }
        if (url.query && url.query.redirect === "fromIdentityProvider") {
            let request = axios.create({ baseURL: identitySettings.authority });
            let authResult = await request.post("/connect/token", makeParams({
                "client_id": "js",
                "code": url.query["code"],
                "redirect_uri": identitySettings["redirect_uri"],
                "grant_type": "authorization_code"
            }));
            console.log(JSON.stringify(authResult.data, null, 4));
            sessionStorage["access_token"] = authResult.data["access_token"];
            window.location = "/";
            return false;
        }
        accessToken = sessionStorage["access_token"] || null;
        return true;
    }
};
