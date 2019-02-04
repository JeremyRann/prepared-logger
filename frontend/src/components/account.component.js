import BackendApi from "../services/backend-api.service";
import authService from "../services/auth.service";

export default {
    name: "Account",
    data () {
        return {
            isLoggedIn: false,
            loading: true,
            userData: []
        };
    },
    async created () {
        this.isLoggedIn = await authService.isLoggedIn();
        if (this.isLoggedIn) {
            this.userData = await BackendApi.get("identity");
        }
        this.loading = false;
    },
    methods: {
    }
};
