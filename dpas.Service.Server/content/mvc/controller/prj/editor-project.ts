/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />

export namespace View {
    export module Prj {
        export class EditorProject extends dpas.Controller {
            public Initialize(): void {
                super.Initialize();

                $("#btnPrjAdd").on("click", function () {
                    $("#modal-prj-name").modal({ dismissible: false });

                    $("#modal-prj-name").modal("open");
                });
            }

            private selectItem: JQuery;
            public Navigate(target: Element): void {
                if (this.selectItem != null) {
                    this.selectItem.removeClass("dpas-tree-active");
                }
                this.selectItem = $(target).addClass("dpas-tree-active");

            }
        }
    }
}

new View.Prj.EditorProject();