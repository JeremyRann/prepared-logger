import oidc from "oidc-client";
new oidc.UserManager().signinSilentCallback().catch((err) => { console.log(err); });
