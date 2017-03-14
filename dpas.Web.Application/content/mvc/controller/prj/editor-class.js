/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var EditorClass = (function (_super) {
            __extends(EditorClass, _super);
            function EditorClass() {
                _super.apply(this, arguments);
            }
            EditorClass.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                $("#editor-tabs").tabs();
                //$("#btnPrjAdd").on("click", function ():void {
                //    impEditor.editor.AddNewItem.call(impEditor.editor);
                //});
            };
            EditorClass.prototype.ApplyLayout = function () {
                var h = $("#editor-content").height() - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);
                var w = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);
            };
            EditorClass.prototype.Navigate = function (target) {
                impEditor.editor.Navigate(target);
            };
            return EditorClass;
        }(dpas.Controller));
        Prj.EditorClass = EditorClass;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var EditorClass = new View.Prj.EditorClass();
//# sourceMappingURL=editor-class.js.map