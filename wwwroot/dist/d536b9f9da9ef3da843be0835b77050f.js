'use strict';

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _reactDom = require('react-dom');

var _App = require('./App');

var _App2 = _interopRequireDefault(_App);

require('bulma/css/bulma.min.css');

require('bootstrap/dist/css/bootstrap.min.css');

require('./site.scss');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

(function () {
  var enterModule = require('react-hot-loader').enterModule;

  enterModule && enterModule(module);
})();

var root = document.createElement('div');
document.body.appendChild(root);
root.id = 'content';

(0, _reactDom.render)(_react2.default.createElement(_App2.default, null), root);
;

(function () {
  var reactHotLoader = require('react-hot-loader').default;

  var leaveModule = require('react-hot-loader').leaveModule;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(root, 'root', 'C:/Users/nullman/source/repos/plx-dev/Tools/Intranet/IntranetApplication/wwwroot/src/index.js');
  leaveModule(module);
})();

;