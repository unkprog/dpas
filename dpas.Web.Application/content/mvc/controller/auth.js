"use strict";
var View;
(function (View) {
    var Auth = (function () {
        function Auth() {
        }
        Auth.prototype.ShowAuthWindow = function () {
            // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
            $('#modal1').openModal();
        };
        return Auth;
    }());
    View.Auth = Auth;
})(View = exports.View || (exports.View = {}));
function Init() {
    (new View.Auth()).ShowAuthWindow();
}
Init();
//# sourceMappingURL=auth.js.map