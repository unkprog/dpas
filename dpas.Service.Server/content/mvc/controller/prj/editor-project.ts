/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />

namespace View {
    export module Prj {
        export class EditorProject extends dpas.Controller {
            public Initialize(): void {
                super.Initialize();

                //$("#btnPrjAdd").on("click", function ():void {
                //    impEditor.editor.AddNewItem.call(impEditor.editor);
                //});
            }

            public Navigate(target: Element): void {
                impEditor.editor.Navigate(target);
            }
        }
    }
}

let editorProject: View.Prj.EditorProject = new View.Prj.EditorProject();