/// <reference path="dpas.d.ts" />
var dpas;
(function (dpas) {
    var Controller = (function () {
        function Controller() {
            dpas.app.navigateSetController(this);
        }
        Controller.prototype.Initialize = function () {
        };
        Controller.prototype.Dispose = function () {
        };
        return Controller;
    }());
    dpas.Controller = Controller;
})(dpas || (dpas = {}));
//# sourceMappingURL=dpas.Controller.js.map