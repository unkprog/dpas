declare var $: any;
declare var navigate: any;
declare var navigateClear: any;
export module View {
    export class Auth {

        
        public Init() {
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
        }

        public Login() {
           
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
        }
    }

}

  
(new View.Auth()).Init();
