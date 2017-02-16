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
        var Editor = (function (_super) {
            __extends(Editor, _super);
            function Editor() {
                _super.apply(this, arguments);
                this.ItemsTree = [];
            }
            Editor.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
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
        }(dpas.Controller));
        Prj.Editor = Editor;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
new View.Prj.Editor();
//# sourceMappingURL=editor.js.map