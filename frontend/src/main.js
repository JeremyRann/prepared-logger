import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import axios from "axios";
import Url from "url-parse";
import oidc from "oidc-client";
import authService from "./services/auth.service";

Vue.config.productionTip = false;
// Hack to allow quick access to internal application objects from a console
window.preparedLoggerGlobals = {
    axios,
    Url,
    oidc
};

authService.initializeApp().then(success => {
    if (success) {
        new Vue({
            router,
            render: h => h(App)
        }).$mount("#app");
    }
});
// TODO: Add a .catch here
