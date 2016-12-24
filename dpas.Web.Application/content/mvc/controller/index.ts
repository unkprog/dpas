declare var $: any;
declare var navigateClear, navigate: any;
declare var showError: any;
export module View {
    export class Index {
        
        public Init() {
            var that = this;
            $("#modal-prj-form").submit(function (e) {
                e.preventDefault();
                $('#modal-prj-name').modal('close');
            });


            $('#modal-prj-name').modal({
                dismissible: false,
                complete: function () { that.NewProject(); }
            });

            $('#btnNewProject').on("click", function () {
                $('#modal-prj-name').modal({
                    dismissible: false,
                    complete: function () { that.NewProject(); }
                });

                $('#modal-prj-name').modal('open');
            });
            $('#btnOpenProject').on("click", function () {
                navigate("/nav/prj/list");
            });


            
        }

        public NewProject() {
            if ('' + $('#prjName').val() === '') {
                return;
            }
            navigateClear();
            var data = { prjName: $('#prjName').val(), prjComment: $('#prjComment').val() };
           
            $.ajax({
                type: "POST", url: location.protocol + '//' + location.host + '/api/prj/create',
                async: true,
                data: data,
                dataType: "json",
                success: function (result) {
                    navigate("/nav/prj/editor");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    showError(thrownError);
                }
            });
        }
    }

}


(new View.Index()).Init();
