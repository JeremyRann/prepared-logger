import BackendApi from "../services/backend-api.service";

export default {
    name: "Dashboard",
    data () {
        return {
            componentLoading: true,
            logs: [],
            addingLog: false,
            newName: ""
        };
    },
    async created () {
        await this.refresh();
        // this.logs = await BackendApi.get("logs");
        // this.componentLoading = false;
    },
    methods: {
        async refresh () {
            this.componentLoading = true;
            this.logs = await BackendApi.get("logs");
            this.componentLoading = false;
        },
        async refresh_Click () {
            await this.refresh();
        },
        add_Click () {
            this.addingLog = true;
            this.newName = "";
        },
        async addLogSave_Click () {
            this.componentLoading = true;
            await BackendApi.post("logs", { name: this.newName });
            this.addingLog = false;
            await this.refresh();
            this.componentLoading = false;
        },
        async deleteLog_Click (logID) {
            if (confirm("Are you sure you want to delete this log?")) {
                this.componentLoading = true;
                await BackendApi.delete("logs/" + logID.toString());
                await this.refresh();
                this.componentLoading = false;
            }
        },
        addLogCancel_Click () {
            this.addingLog = false;
        }
    }
};
