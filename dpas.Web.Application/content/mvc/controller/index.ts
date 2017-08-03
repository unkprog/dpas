/// <reference path="../../ts/jquery-3.1.1.d.ts" />
/// <reference path="../dpas.d.ts" />
/// <reference path="../dpas.controller.ts" />

export namespace View {

    export class Index extends dpas.Controller {
        private modalNewProject: JQuery;

        public Initialize(): void {
            super.Initialize();
          
            var that: Index = this;
           
            

            $('#btnNewProject').on("click", function () {
                //$('#modal-prj-name').modal({
                //    dismissible: false,
                //    complete: function () { that.NewProject(); }
                //});

                that.modalNewProject.modal('open');
            });

            $("#btnOpenProject").on("click", function () {
                dpas.app.navigate({ url: "/nav/prj/list" });
            });

            //$('#btnTEst').on("click", function () {
            //    var data = { prjCode: "id" };
            //    dpas.app.postJson({
            //        url: '/api/prj/current', data: data,
            //        success: function (result:any):any {

            //            dpas.app.postJson({
            //                url: '/api/prj/editor',
            //                data: { command: "prjtree" },
            //                success: function (result) {
            //                    //showError(JSON.stringify(result.data));
            //                }
            //            });
            //        }
            //    });
            //});
        }

        public Dispose(): void {
            super.Dispose();
        }
    }

}

new View.Index();

