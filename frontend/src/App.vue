<template>
    <div id="app">
        <div id="nav">
            <router-link to="/">Home</router-link> |
            <span v-if="isLoggedIn === false" @click="login_Click">Log in</span>
            <span v-if="isLoggedIn === true" @click="logout_Click">Log out</span>
        </div>
        <router-view/>
    </div>
</template>

<script>
import authService from "./services/auth.service";

console.log(authService);

export default {
    name: "App",
    data () {
        return {
            isLoggedIn: null
        };
    },
    async created () {
        this.isLoggedIn = await authService.isLoggedIn();
    },
    methods: {
        async login_Click () {
            await authService.logIn();
        },
        async logout_Click () {
            await authService.logOut();
        }
    }
};
</script>

<style lang="scss">
#app {
    font-family: 'Avenir', Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: left;
    color: #2c3e50;
}
#nav {
    text-align: center;
    padding: 30px;
    a,span {
        font-weight: bold;
        color: #2c3e50;
        cursor: pointer;
        &.router-link-exact-active {
            color: #42b983;
        }
    }
}
</style>
