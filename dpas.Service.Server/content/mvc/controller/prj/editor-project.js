"use strict";
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var EditorProject = (function () {
            function EditorProject() {
            }
            EditorProject.prototype.Init = function () {
                var that = this;
                $('#btnPrjAdd').on("click", function () {
                    $('#modal-prj-name').modal({
                        dismissible: false,
                    });
                    $('#modal-prj-name').modal('open');
                });
            };
            return EditorProject;
        }());
        Prj.EditorProject = EditorProject;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
(new View.Prj.EditorProject()).Init();
//# sourceMappingURL=editor-project.js.map