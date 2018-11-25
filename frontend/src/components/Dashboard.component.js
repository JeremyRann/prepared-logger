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
        this.logs = await BackendApi.get("log");
        this.componentLoading = false;
    },
    methods: {
        add_Click () {
            this.addingLog = true;
            this.newName = "";
        },
        async addLogSave_Click () {
            this.componentLoading = true;
            await BackendApi.post("log", { name: this.newName });
            this.componentLoading = false;
            this.addingLog = false;
        },
        addLogCancel_Click () {
            this.addingLog = false;
        }
    }
};
