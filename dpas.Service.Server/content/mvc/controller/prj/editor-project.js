var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />
var impEditor = View.Prj.Editor;
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var EditorProject = (function (_super) {
            __extends(EditorProject, _super);
            function EditorProject() {
                _super.apply(this, arguments);
            }
            EditorProject.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                $("#btnPrjAdd").on("click", function () {
                    impEditor.editor.AddNewItem.call(impEditor.editor);
                });
            };
            EditorProject.prototype.Navigate = function (target) {
                if (impEditor.editor.selectedItem != null) {
                    impEditor.editor.selectedItem.removeClass("dpas-tree-active");
                }
                impEditor.editor.selectedItem = $(target).addClass("dpas-tree-active");
            };
            return EditorProject;
        }(dpas.Controller));
        Prj.EditorProject = EditorProject;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var editorProject = new View.Prj.EditorProject();
//# sourceMappingURL=editor-project.js.map