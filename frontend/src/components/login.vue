<template>
    <div>
        <h1>Log in</h1>
        <div v-if="componentLoading">Loading...</div>
        <div v-if="!componentLoading">
        <form @submit.prevent="form_Submit">
            <label>
                <span>User Name:</span>
                <input v-model="userName" autofocus="autofocus" />
            </label>
            <label>
                <span>Password:</span>
                <input type="password" v-model="password" />
            </label>
            <button type="submit">Log In</button>
        </form>
        </div>
    </div>
</template>

<script>
import LoginComponent from "./login.component";
export default LoginComponent;
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
label {
    display: block;
    margin-bottom: 0.5em;
}

label > span {
    display: inline-block;
    min-width: 6em;
}
</style>
