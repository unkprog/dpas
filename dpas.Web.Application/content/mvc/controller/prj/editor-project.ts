/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />

export namespace View {
    export module Prj {
        export class EditorProject extends dpas.Controller {
            public Initialize(): void {
                super.Initialize();

                var that = this;
                $('#btnPrjAdd').on("click", function () {
                    $('#modal-prj-name').modal({
                        dismissible: false,
                        //complete: function () { that.NewProject(); }
                    });

                    $('#modal-prj-name').modal('open');
                });
            }
        }
    }
}

new View.Prj.EditorProject();