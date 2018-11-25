const process = require("process");
module.exports = {
    devServer: {
        proxy: {
            "/api": {
                target: process.env.API_LOCATION || "https://localhost:5001",
                ws: true,
                changeOrigin: true
            }
        }
    }
};
