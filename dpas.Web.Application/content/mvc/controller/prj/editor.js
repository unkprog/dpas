"use strict";
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var Editor = (function () {
            function Editor() {
                this.This = this;
            }
            Editor.prototype.Init = function () {
                var that = this;
                $("#editor-menu-tree-view").treemenu({ delay: 300 }); //.openActive();
                $('ul.tabs').tabs();
                $(window).resize(function () {
                    that.ApplyLayout();
                });
                that.ApplyLayout();
                that.TreeProjectLoad();
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
            Editor.prototype.TreeProjectLoad = function () {
                var that = this;
                dpas.app.postJson({
                    url: '/api/prj/editor',
                    data: { command: "prjtree" },
                    success: function (result) {
                        that.SetupTreeProject(result.data);
                    }
                });
            };
            Editor.prototype.SetupTreeProject = function (dataTreeProject) {
                var ids = [], id = 0;
                var drawItemTree = function (curItem) {
                    var isReference = false || curItem.type === 0 || curItem.type === 3 || curItem.type === 4;
                    var result = '<li>';
                    //if (isReference) {
                    result += '<a id="';
                    result += curItem.Path;
                    result += '" data-id="';
                    result += ids.length.toString();
                    result += '">';
                    ids.push(curItem);
                    //}
                    //else
                    //    result += '<span>';
                    result += curItem.Name;
                    result += '</a>'; //isReference ? '</a>' : '</span>';
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
                var that = this;
                that['ids'] = ids;
                for (i = 0, icount = ids.length; i < icount; i++) {
                    var elItem = ids[i];
                    var el = $(document.getElementById(elItem.Path));
                    //el.elItem = elItem;
                    el.click(function () {
                        alert(that['ids'][$(this).data('id')].Path);
                    });
                }
            };
            return Editor;
        }());
        Prj.Editor = Editor;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
(new View.Prj.Editor()).Init();
//# sourceMappingURL=editor.js.map