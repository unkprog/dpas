var dpas;
(function (dpas) {
    var Controller = (function () {
        function Controller() {
            dpas.app.navigateSetController(this);
        }
        Controller.prototype.Initialize = function () {
        };
        Controller.prototype.ApplyLayout = function () {
        };
        Controller.prototype.Dispose = function () {
            dpas.app.navigateRemoveController(this);
        };
        Controller.prototype.Navigate = function (target) {
        };
        return Controller;
    }());
    dpas.Controller = Controller;
})(dpas || (dpas = {}));
//# sourceMappingURL=dpas.Controller.js.map