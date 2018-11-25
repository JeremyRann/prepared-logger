const axios = require("axios");

export default {
    async post (url, data) {
        url = "/api/" + url;
        return (await axios.post(url, data)).data;
    },
    async get (url) {
        url = "/api/" + url;
        return (await axios.get(url)).data;
    }
};
