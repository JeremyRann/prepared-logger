import oidc from "oidc-client";
import Url from "url-parse";

let userManager = new oidc.UserManager({
    authority: "http://localhost:5001",
    client_id: "js",
    redirect_uri: "http://localhost:8080?redirect=fromIdentityProvider",
    response_type: "code",
    scope: "openid profile PreparedLogger",
    post_logout_redirect_uri: "http://localhost:8080"
});

let user = null;

export default {
    async logIn () {
        await userManager.signinRedirect();
    },
    async isLoggedIn () {
        let user = await userManager.getUser();
        console.log(["user", user]);
        return !!user;
    },
    isGuest () {
        return user === null;
    },
    async logOut () {
        await userManager.signoutRedirect();
    },
    async checkForRedirect () {
        let url = new Url(window.location.href, true);
        if (url.query && url.query.redirect === "fromIdentityProvider") {
            await new oidc.UserManager({ response_mode: "query" }).signinRedirectCallback();
            window.location = "/";
            return true;
        }
        return false;
    },
    async getAccessToken () {
        return new Promise(resolve => {
            if (user === null) {
                userManager.signinRedirect();
            }
            else {
                resolve(user.access_token);
            }
        });
    },
    async initializeApp () {
        // Go back to the app if this is an ID provider redirect
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
