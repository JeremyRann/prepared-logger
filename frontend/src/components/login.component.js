// import BackendApi from "../services/backend-api.service";

export default {
    name: "Login",
    data () {
        return {
            userName: "admin",
            password: null,
            componentLoading: false
        };
    },
    methods: {
        async form_Submit () {
            console.log("Submitting...");
        }
    }
};
