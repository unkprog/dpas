"use strict";
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var Editor = (function () {
            function Editor() {
                this.ItemsTree = [];
                this.That = this;
            }
            Editor.prototype.Init = function () {
                var that = this;
                var content = $("#editor-content");
                dpas.app.navigateSetContent('/prj', content);
                $("#editor-menu-tree-view").treemenu({ delay: 300 }); //.openActive();
                $('ul.tabs').tabs();
                $(window).resize(function () {
                    that.ApplyLayout();
                });
                that.ApplyLayout();
                that.TreeProjectLoad(that);
                return this;
            };
            Editor.prototype.ApplyLayout = function () {
                var h = window.innerHeight - $('.navbar-fixed').height() - 22;
                $('#editor-menu').height(h);
                $('#editor-menu-tree').height(h - 80);
                $('#editor-content').height(h);
                h = h - $('#editor-tabs').height();
                $('#editor-designer-view').height(h);
                $('#editor-code-view').height(h);
                $('#code-view-textarea').height(h);
                var w = window.innerWidth - $('#editor-menu').width() - 17;
                $('#editor-content').width(w);
                $('#code-view-textarea').width(w - 4);
            };
            Editor.prototype.TreeProjectLoad = function (That) {
                var that = That;
                dpas.app.postJson({
                    url: '/api/prj/editor',
                    data: { command: "prjtree" },
                    success: function (result) {
                        that.SetupTreeProject(that, result.data);
                    }
                });
            };
            Editor.prototype.SetupTreeProject = function (That, dataTreeProject) {
                That.ItemsTree = [];
                var id = 0;
                var drawItemTree = function (curItem) {
                    var isReference = false || curItem.Type === 0 || curItem.Type === 3 || curItem.Type === 4;
                    var result = '<li>';
                    result += '<a id="';
                    result += curItem.Path;
                    result += '" class="ajax" data-id="';
                    result += curItem.Code;
                    result += '" href="/prj/editor-project';
                    result += curItem.Type === 0 ? '?project=' + curItem.Name : '/' + curItem.Path;
                    result += '">';
                    That.ItemsTree.push(curItem);
                    That.ItemsTree[curItem.Code] = curItem;
                    result += curItem.Name;
                    result += '</a>';
                    if (curItem.Items !== undefined) {
                        for (var i = 0, icount = curItem.Items.length; i < icount; i++) {
                            result += '<ul>';
                            result += drawItemTree(curItem.Items[i]);
                            result += '</ul>';
                        }
                    }
                    result += '</li>';
                    return result;
                };
                var elsStr = '';
                var i = 0, icount = dataTreeProject.length;
                for (; i < icount; i++) {
                    elsStr += drawItemTree(dataTreeProject[i]);
                }
                $("#editor-menu-tree-view").html(elsStr).treemenu({ delay: 300 });
            };
            return Editor;
        }());
        Prj.Editor = Editor;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
exports.View_Prj_Editor = (new View.Prj.Editor()).Init();
//# sourceMappingURL=editor.js.map