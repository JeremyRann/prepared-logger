import authService from "./auth.service";
import axios from "axios";

export default {
    async baseRequest () {
        let requestObject = {
            baseURL: "/api/",
            headers: {}
        };
        if (!authService.isGuest()) {
            let accessToken = await authService.getAccessToken();
            requestObject.headers["Authorization"] = "Bearer " + accessToken;
        }
        return axios.create(requestObject);
    },
    async post (url, data) {
        let request = await this.baseRequest();
        return (await request.post(url, data)).data;
    },
    async get (url) {
        let request = await this.baseRequest();
        return (await request.get(url)).data;
    },
    async delete (url) {
        let request = await this.baseRequest();
        return (await request.delete(url)).data;
    }
};
