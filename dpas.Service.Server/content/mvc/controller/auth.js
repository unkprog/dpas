"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var View;
(function (View) {
    var Auth = (function () {
        function Auth() {
        }
        Auth.prototype.Init = function () {
            var that = this;
            $("form").submit(function (e) {
                e.preventDefault();
                $('#modal-auth').modal('close');
            });
            $('#modal-auth').modal({
                dismissible: false,
                complete: function () { that.Login(); }
            });
            $('#modal-auth').modal('open');
        };
        Auth.prototype.Login = function () {
            navigateClear();
            var data = { login: $('#login').val(), password: $('#password').val() };
            //var formData = new FormData($('form')[0]);
            //formData.append("login", document.getElementById('i-login').value);
            //formData.append("password", hashObj.getHash("SHA-256", "HEX"));
            $.ajax({
                type: "POST", url: location.protocol + '//' + location.host + '/api/auth',
                async: true,
                data: data,
                dataType: "json",
                success: function (result) {
                    navigate("/nav/curpage");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    navigate("/nav/curpage");
                    //showError("Авторизация", thrownError);
                    //app.hideLoading();
                }
            });
        };
        return Auth;
    }());
    View.Auth = Auth;
})(View = exports.View || (exports.View = {}));
(new View.Auth()).Init();
//# sourceMappingURL=auth.js.map