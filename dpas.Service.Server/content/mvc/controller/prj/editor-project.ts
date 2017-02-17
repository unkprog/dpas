/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />
import impEditor = View.Prj.Editor;

namespace View {
    export module Prj {
        export class EditorProject extends dpas.Controller {
            public Initialize(): void {
                super.Initialize();

                $("#btnPrjAdd").on("click", function () {
                    impEditor.editor.AddNewItem.call(impEditor.editor);
                });
            }

            public Navigate(target: Element): void {
                if (impEditor.editor.selectedItem != null) {
                    impEditor.editor.selectedItem.removeClass("dpas-tree-active");
                }
                impEditor.editor.selectedItem = $(target).addClass("dpas-tree-active");
            }
        }
    }
}

let editorProject: View.Prj.EditorProject = new View.Prj.EditorProject();