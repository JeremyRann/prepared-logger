<template>
    <div>
        <div v-if="componentLoading">Loading...</div>
        <div v-if="!componentLoading">
            <h1>Welcome to Prepared Logger</h1>
            <h2>Logs</h2>
            <ul>
                <li v-for="log in logs" :key="log.logID">{{log.name}}</li>
            </ul>
            <button v-if="!addingLog" type="button" @click="add_Click">Add</button>
            <div v-if="addingLog">
                <form>
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
