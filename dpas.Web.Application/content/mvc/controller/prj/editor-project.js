/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
                var that = this;
                $('#btnPrjAdd').on("click", function () {
                    $('#modal-prj-name').modal({
                        dismissible: false,
                    });
                    $('#modal-prj-name').modal('open');
                });
            };
            return EditorProject;
        }(dpas.Controller));
        Prj.EditorProject = EditorProject;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
new View.Prj.EditorProject();
//# sourceMappingURL=editor-project.js.map