"use strict";
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var Editor = (function () {
            function Editor() {
            }
            Editor.prototype.Init = function () {
                var that = this;
                $("#btnMainMenu").on("click", function (event) {
                    navigate("/nav/index");
                });
                $(".tree").treemenu({ delay: 300 }); //.openActive();
                $('ul.tabs').tabs();
            };
            return Editor;
        }());
        Prj.Editor = Editor;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
(new View.Prj.Editor()).Init();
//# sourceMappingURL=editor.js.map