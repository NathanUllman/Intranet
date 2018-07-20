const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');

const DIST_PATH = './wwwroot/dist/';

module.exports = {
    mode: 'development',
    entry: {
        'main-client': './wwwroot/src/index'
    },
    output: {
        path: path.join(__dirname, 'wwwroot/dist'),
        filename: 'bundle.js',
        publicPath: DIST_PATH // publicPath must be included for webpack-hot-middleware to work
    },
    devtool: 'inline-source-map',
    module: {
        rules: [
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader']
            },
            {
                test: /\.(scss|sass)$/,
                use: [
                    "style-loader", // creates style nodes from JS strings
                    "css-loader", // translates CSS into CommonJS
                    "sass-loader" // compiles Sass to CSS
                ]
            },
            {
                test: /\.(png|svg|jpg|gif)$/,
                use: [
                    'file-loader'
                ]
            },
            {
                test: /\.(ttf|woff|woff2|eot)$/,
                use: [
                    'file-loader'
                ]
            },
            {
                exclude: /node_modules|packages/,
                test: /\.js$/,
                use: 'babel-loader'
            }
        ]
    },
    plugins: [new HtmlWebpackPlugin(), new webpack.NamedModulesPlugin()]
}