import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import axios from "axios";

Vue.config.productionTip = false;
// Hack to allow quick access to internal application objects from a console
window.preparedLoggerGlobals = {
    axios
};

new Vue({
    router,
    render: h => h(App)
}).$mount("#app");
