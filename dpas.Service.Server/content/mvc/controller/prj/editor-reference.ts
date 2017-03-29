/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />

namespace View {
    export module Prj {
        export class EditorReference extends dpas.Controller {
            public Initialize(): void {
                super.Initialize();

            }

            public Navigate(target: Element): void {
                impEditor.editor.Navigate(target);
            }
        }
    }
}

let editorReference: View.Prj.EditorReference = new View.Prj.EditorReference();