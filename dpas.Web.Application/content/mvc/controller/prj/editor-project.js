/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var EditorProject = (function (_super) {
            __extends(EditorProject, _super);
            function EditorProject() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            EditorProject.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                //$("#btnPrjAdd").on("click", function ():void {
                //    impEditor.editor.AddNewItem.call(impEditor.editor);
                //});
            };
            EditorProject.prototype.Navigate = function (target) {
                impEditor.editor.Navigate(target);
            };
            return EditorProject;
        }(dpas.Controller));
        Prj.EditorProject = EditorProject;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var editorProject = new View.Prj.EditorProject();
//# sourceMappingURL=editor-project.js.map