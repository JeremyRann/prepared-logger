import oidc from "oidc-client";

console.log(oidc);

let userManager = new oidc.UserManager({
    authority: "http://localhost:5001",
    client_id: "js",
    redirect_uri: "http://localhost:8080?callback",
    response_type: "code",
    scope: "openid profile PreparedLogger",
    post_logout_redirect_uri: "http://localhost:8080"
});

export default {
    async logIn () {
        await userManager.signinRedirect();
    },
    async isLoggedIn () {
        let user = await userManager.getUser();
        return !!user;
    },
    async logOut () {
        await userManager.signoutRedirect();
    }
};
