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
        var EditorData = (function (_super) {
            __extends(EditorData, _super);
            function EditorData() {
                _super.apply(this, arguments);
            }
            EditorData.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
            };
            EditorData.prototype.Navigate = function (target) {
                impEditor.editor.Navigate(target);
            };
            return EditorData;
        }(dpas.Controller));
        Prj.EditorData = EditorData;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var editorData = new View.Prj.EditorData();
//# sourceMappingURL=editor-data.js.map