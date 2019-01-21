<template>
    <div v-if="!loading">
        <h1>Account</h1>
        <p v-if="!isLoggedIn">User is not loged in</p>
        <div v-if="isLoggedIn">
            <ul v-for="item in userData" :key="item.type">
                <li>
                    <span>{{item.type}}</span>:
                    <span>{{item.value}}</span>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
import AccountComponent from "./account.component";
export default AccountComponent;
</script>

<style scoped lang="scss">
</style>
