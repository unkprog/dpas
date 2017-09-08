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
                that.modalNewProject.modal('open');
            });

            $("#btnOpenProject").on("click", function () {
                dpas.app.navigate({ url: "/nav/prj/list" });
            });

        }

        public Dispose(): void {
            super.Dispose();
        }
    }

}

new View.Index();

