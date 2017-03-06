/// <reference path="../../../ts/materialize.d.ts" />
/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
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
                _super.call(this);
                this.ItemsTree = [];
                Editor.editor = this;
            }
            Editor.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                var that = this;
                that.typeSelect = $("select").material_select();
                that.dialogAdd = $("#editor-add").modal({ dismissible: false, complete: function () { that.AddNewItemCompleted(that); } });
                var content = $("#editor-content");
                dpas.app.navigateSetContent("/prj", content);
                $("#editor-menu-tree-view").treemenu({ delay: 300 });
                that.TreeProjectLoad(that);
                $("#editor-add-form").submit(function (e) {
                    e.preventDefault();
                    that.dialogAdd.modal("close");
                });
            };
            Editor.prototype.ApplyLayout = function () {
                var h = window.innerHeight - $(".navbar-fixed").height() - 22;
                $("#editor-menu").height(h);
                $("#editor-menu-tree").height(h - 80);
                $("#editor-content").height(h);
                h = h - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);
                var w = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);
            };
            Editor.prototype.TreeProjectLoad = function (That) {
                dpas.app.postJson({
                    url: "/api/prj/editor",
                    data: { command: "prjtree" },
                    success: function (result) {
                        That.SetupTreeProject(That, result.data);
                    }
                });
            };
            Editor.prototype.DrawItemTree = function (That, curItem) {
                var result = "<li";
                if (!(curItem.Items !== undefined && curItem.Items.length > 0)) {
                    result += " class=\"dpas-tree-empty\"";
                }
                result += "><span class=\"dpas-toggler\"></span>";
                result += "<a id=\"";
                result += curItem.Path;
                result += "\" class=\"ajax\" data-id=\"";
                result += curItem.Code;
                result += "\" href=\"/prj/editor-project";
                result += curItem.Type === 0 ? "?project=" + curItem.Name : "/" + curItem.Path;
                result += "\">";
                That.SaveItemTree(curItem);
                result += curItem.Name;
                result += "</a>";
                if (curItem.Items !== undefined) {
                    for (var i = 0, icount = curItem.Items.length; i < icount; i++) {
                        result += "<ul class=\"dpas-treemenu\">";
                        //result += "<ul>";
                        result += That.DrawItemTree(That, curItem.Items[i]);
                        result += "</ul>";
                    }
                }
                result += "</li>";
                return result;
            };
            Editor.prototype.SaveItemTree = function (curItem) {
                this.ItemsTree.push(curItem);
                this.ItemsTree[curItem.Code] = curItem;
            };
            Editor.prototype.AppendItemTree = function (That, result) {
                var itemHtml = That.DrawItemTree(That, result.data);
                if (result.data.Type === 1 || result.data.Type === 2)
                    itemHtml = "<ul class=\"dpas-treemenu\">" + itemHtml + "</ul>";
                //let toggler: JQuery = That.selectedItem.find('.dpas-toggler');
                //if (toggler.length === 0)
                //    That.selectedItem.add('<span class="dpas-toggler" ></span>');
                That.selectedItem.parent().removeClass("dpas-tree-empty").addClass("dpas-tree-opened").addClass(".dpas-tree-active");
                That.selectedItem.parent().append(itemHtml);
                That.SaveItemTree(result.data);
                //That.selectedItem.addClass("dpas-tree-active");
            };
            Editor.prototype.SetupTreeProject = function (That, dataTreeProject) {
                That.ItemsTree = [];
                var elsStr = "";
                var i = 0, icount = dataTreeProject.length;
                for (; i < icount; i++) {
                    elsStr += That.DrawItemTree(That, dataTreeProject[i]);
                }
                $("#editor-menu-tree-view").html(elsStr).treemenu({ delay: 300 });
            };
            Editor.prototype.AddNewItem = function () {
                if (this.selectedItem == null) {
                    return;
                }
                this.dialogAdd.modal("open");
            };
            Editor.prototype.AddNewItemCompleted = function (That) {
                var name = "" + $("#editor-add-name").val();
                if (name === "") {
                    return;
                }
                var data = {
                    command: "additem",
                    Type: $("#editor-add-type").val(), Name: name, Description: $("#editor-add-description").val(),
                    Parent: That.selectedItem.attr("id")
                };
                dpas.app.postJson({
                    url: "/api/prj/editor", data: data,
                    success: function (result) {
                        if (result.result === true) {
                            Editor.editor.AppendItemTree(Editor.editor, result);
                        }
                        else {
                            dpas.app.showError(result.error);
                        }
                    }
                });
            };
            return Editor;
        }(dpas.Controller));
        Prj.Editor = Editor;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var editor = new View.Prj.Editor();
//# sourceMappingURL=editor.js.map