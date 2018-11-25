<template>
    <div>
        <h1>Welcome to Prepared Logger</h1>
        <div v-if="componentLoading">Loading...</div>
        <div v-if="!componentLoading">
            <h2>Logs <button type="button" @click="refresh_Click">Refresh</button></h2>
            <p v-if="!logs || logs.length < 1">(no logs to show)</p>
            <ul v-if="logs && logs.length > 0">
                <li v-for="log in logs" :key="log.logID">
                    {{log.name}}
                    <button type="button" @click="deleteLog_Click(log.logID)">Delete</button>
                </li>
            </ul>
            <button v-if="!addingLog" type="button" @click="add_Click">Add</button>
            <div v-if="addingLog">
                <form onsubmit="return false;">
                    <label>
                        Name:
                        <input v-model="newName" />
                    </label>
                </form>
                <button type="button" @click="addLogSave_Click">Save</button>
                <button type="button" @click="addLogCancel_Click">Cancel</button>
            </div>
        </div>
    </div>
</template>

<script>
import DashboardComponent from "./Dashboard.component";
export default DashboardComponent;
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
</style>
