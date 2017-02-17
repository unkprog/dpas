/// <reference path="../dpas.d.ts" />
/// <reference path="../dpas.controller.ts" />

export namespace View {

    export class Index extends dpas.Controller {

        public Initialize(): void {
            super.Initialize();
          
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
                dpas.app.navigate({ url: "/nav/prj/list" });
            });

            $('#btnTEst').on("click", function () {
                var data = { prjCode: "id" };
                dpas.app.postJson({
                    url: '/api/prj/current', data: data,
                    success: function (result) {

                        dpas.app.postJson({
                            url: '/api/prj/editor',
                            data: { command: "prjtree" },
                            success: function (result) {
                                //showError(JSON.stringify(result.data));
                            }
                        });


                    }
                });

            });
        }

        public Dispose(): void {
            super.Dispose();
        }

        public NewProject() {
            //if ('' + $('#prjName').val() === '') {
            //    return;
            //}
            //dpas.app.navigateClear();
            //var data = { prjName: $('#prjName').val(), prjDescription: $('#prjDescription').val() };

            //dpas.app.postJson({
            //    url: '/api/prj/create', data: data,
            //    success: function (result) {
            //        if (result.result == true)
            //            dpas.app.navigate("/nav/prj/editor?prj=" + result.project.Code);
            //        else
            //            dpas.app.showError(result.error);
            //    }
            //});

            ////$.ajax({
            ////    type: "POST", url: location.protocol + '//' + location.host + '/api/prj/create',
            ////    async: true,
            ////    data: data,
            ////    dataType: "json",
            ////    success: function (result) {
            ////        if (result.resut == true)
            ////            navigate("/nav/prj/editor");
            ////        else
            ////            showError(result.error);
            ////    },
            ////    error: function (xhr, ajaxOptions, thrownError) {
            ////        showError(thrownError);
            ////    }
            ////});
        }
    }

}

new View.Index();

//# sourceMappingURL=mvc/controller/index.js.map
