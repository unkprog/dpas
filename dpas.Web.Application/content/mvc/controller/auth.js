"use strict";
var View;
(function (View) {
    var Auth = (function () {
        function Auth() {
        }
        Auth.prototype.Init = function () {
            $('#modal-auth').modal({
                dismissible: false
            });
            $('#modal-auth').modal('open');
        };
        return Auth;
    }());
    View.Auth = Auth;
})(View = exports.View || (exports.View = {}));
(new View.Auth()).Init();
//# sourceMappingURL=auth.js.map