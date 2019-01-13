import Vue from "vue";
import Router from "vue-router";
import Dashboard from "./components/Dashboard.vue";
import Login from "./components/login.vue";

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: "/",
            name: "home",
            component: Dashboard
        },
        {
            path: "/login",
            name: "login",
            component: Login
        }
    ]
});
