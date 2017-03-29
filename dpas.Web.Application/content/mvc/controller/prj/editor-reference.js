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
        var EditorReference = (function (_super) {
            __extends(EditorReference, _super);
            function EditorReference() {
                _super.apply(this, arguments);
            }
            EditorReference.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
            };
            EditorReference.prototype.Navigate = function (target) {
                impEditor.editor.Navigate(target);
            };
            return EditorReference;
        }(dpas.Controller));
        Prj.EditorReference = EditorReference;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var editorReference = new View.Prj.EditorReference();
//# sourceMappingURL=editor-reference.js.map