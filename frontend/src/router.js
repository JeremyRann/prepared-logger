import Vue from "vue";
import Router from "vue-router";
import Dashboard from "./components/Dashboard.vue";
import Account from "./components/account.vue";

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: "/",
            name: "home",
            component: Dashboard
        },
        {
            path: "/account",
            name: "account",
            component: Account
        }
    ]
});
