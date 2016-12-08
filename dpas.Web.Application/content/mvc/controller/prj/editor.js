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
                that.ApplyLayout();
                $(window).resize(function () {
                    that.ApplyLayout();
                });
            };
            Editor.prototype.ApplyLayout = function () {
                var h = window.innerHeight - $('.navbar-fixed').height();
                $('#editor-menu').height(h);
                $('#editor-menu-tree').height(h - 64);
                $('#editor-content').height(h);
                h = h - $('#editor-tabs').height();
                $('#editor-designer-view').height(h);
                $('#editor-code-view').height(h);
                $('#code-view-textarea').height(h);
                var w = window.innerWidth - $('#editor-menu').width();
                $('#editor-content').width(w);
                $('#code-view-textarea').width(w - 4);
            };
            return Editor;
        }());
        Prj.Editor = Editor;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
(new View.Prj.Editor()).Init();
//# sourceMappingURL=editor.js.map