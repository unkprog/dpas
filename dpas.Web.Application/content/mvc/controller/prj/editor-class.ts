/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />

namespace View {
    export module Prj {
        export class EditorClass extends dpas.Controller {
            public Initialize(): void {
                super.Initialize();
                $("#editor-tabs").tabs();
                //$("#btnPrjAdd").on("click", function ():void {
                //    impEditor.editor.AddNewItem.call(impEditor.editor);
                //});
            }

            public ApplyLayout(): void {
                let h: number = $("#editor-content").height() - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);

                let w: number = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);
            }


            public Navigate(target: Element): void {
                impEditor.editor.Navigate(target);
            }
        }
    }
}

let EditorClass: View.Prj.EditorClass = new View.Prj.EditorClass();