/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />

namespace View {
    export module Prj {
        export class EditorData extends dpas.Controller {
            public Initialize(): void {
                super.Initialize();

            }

            public Navigate(target: Element): void {
                impEditor.editor.Navigate(target);
            }
        }
    }
}

let editorData: View.Prj.EditorData = new View.Prj.EditorData();