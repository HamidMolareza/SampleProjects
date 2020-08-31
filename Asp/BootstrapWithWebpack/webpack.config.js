const path = require("path");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        bootstrap_css: "./node_modules/bootstrap/dist/css/bootstrap.min.css",
        jquery: "./node_modules/jquery/dist/jquery.min.js",
        bootstrap_js: "./node_modules/bootstrap/dist/js/bootstrap.bundle.min.js",
        jquery_validate: "./node_modules/jquery-validation/dist/jquery.validate.min.js",
        jquery_validate: "./node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js",
        site: "./ClientSource/js/site.js"
    },
    output: {
        path: path.resolve(__dirname, "wwwroot","wp"),
        filename: "js/[name].js",
        publicPath: "/"
    },
    resolve: {
        extensions: [".js"]
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [MiniCssExtractPlugin.loader, "css-loader"]
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin(),
        new MiniCssExtractPlugin({
            filename: "css/[name].css"
        })
    ]
};