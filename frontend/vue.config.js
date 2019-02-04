const process = require("process");
module.exports = {
    // Disable HMR
    // https://stackoverflow.com/a/52766924
    chainWebpack: config => {
        config.module
            .rule("vue")
            .use("vue-loader")
            .loader("vue-loader")
            .tap(options => {
                options.hotReload = false
                return options
            })
    },
    pages: {
        index: {
            entry: "src/main.js"
        },
        "silent-refresh": {
            entry: "src/silent-refresh.js"
        }
    },
    devServer: {
        proxy: {
            "/api": {
                target: process.env.API_LOCATION || "http://localhost:5000",
                ws: true,
                changeOrigin: true
            }
        }
    }
};
