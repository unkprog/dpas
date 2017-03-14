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
                that.typeSelect = $("#editor-add-type");
                that.typeSelect.material_select();
                that.dialogAdd = $("#editor-add").modal({ dismissible: false, complete: function () { that.AddNewItemCompleted(that); } });
                var content = $("#editor-content");
                dpas.app.navigateSetContent("/prj", content);
                $("#editor-menu-tree-view").treemenu({ delay: 300 });
                that.TreeProjectLoad(that);
                $("#editor-add-form").submit(function (e) {
                    e.preventDefault();
                    that.dialogAdd.modal("close");
                });
                that.buttonAdd = $("#editor-menu-button-add");
                that.buttonDel = $("#editor-menu-button-del");
                that.SelectApply(null);
                that.buttonAdd.on("click", function () {
                    that.AddNewItem();
                });
                that.buttonDel.on("click", function () {
                    that.DeleteItem();
                });
            };
            Editor.prototype.ApplyLayout = function () {
                var h = window.innerHeight - $(".navbar-fixed").height() - 22;
                $("#editor-menu").height(h);
                $("#editor-menu-tree").height(h - 90 - $(".editor-menu-buttons").height());
                $("#editor-content").height(h);
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
                curItem.PathId = curItem.Path.replace(new RegExp("/", "g"), "_");
                var result = "<li";
                if (!(curItem.Items !== undefined && curItem.Items.length > 0) && !(curItem.Type === 1 || curItem.Type === 2)) {
                    result += " class=\"dpas-tree-empty\"";
                }
                result += "><span class=\"dpas-toggler\"></span>";
                //if (curItem.Type == 0) {
                //    result += "<i class=\"small material-icons left\" style=\"margin-right:5px\">create_new_folder</i>";
                //}
                result;
                result += "<a id=\"";
                result += curItem.PathId;
                result += "\" class=\"ajax\" href=\"/prj/";
                result += curItem.Type === 0 ? "editor-project" : curItem.Type === 1 ? "editor-reference" : curItem.Type === 2 ? "editor-data" : "editor-class";
                result += curItem.Type === 0 ? "?project=" + curItem.Name : "?projectitem=" + curItem.PathId;
                result += "\">";
                That.SaveItemTree(curItem);
                result += curItem.Name;
                result += "</a>";
                if (curItem.Items !== undefined) {
                    for (var i = 0, icount = curItem.Items.length; i < icount; i++) {
                        result += "<ul class=\"dpas-treemenu\">";
                        result += That.DrawItemTree(That, curItem.Items[i]);
                        result += "</ul>";
                    }
                }
                result += "</li>";
                return result;
            };
            Editor.prototype.SaveItemTree = function (curItem) {
                this.ItemsTree.push(curItem);
                this.ItemsTree[curItem.PathId] = curItem;
            };
            Editor.prototype.AppendItemTree = function (That, result) {
                var itemHtml = That.DrawItemTree(That, result.data);
                if (result.data.Type === 1 || result.data.Type === 2)
                    itemHtml = "<ul class=\"dpas-treemenu\">" + itemHtml + "</ul>";
                That.selectedItem.parent().removeClass("dpas-tree-empty").addClass("dpas-tree-opened").addClass(".dpas-tree-active");
                That.selectedItem.parent().append(itemHtml);
                That.SelectApply($("#" + result.data.PathId));
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
            Editor.prototype.Navigate = function (target) {
                Editor.editor.SelectApply($(target));
            };
            Editor.prototype.SelectApply = function (item) {
                if (this.isSelected()) {
                    this.selectedItem.removeClass("dpas-tree-active");
                }
                this.selectedItem = item;
                if (this.isSelected()) {
                    this.selectedItem.addClass("dpas-tree-active");
                    var itemData = this.ItemsTree[this.selectedItem.attr("id")];
                    if (itemData === undefined || itemData === null || itemData.Type === 0) {
                        this.buttonDel.addClass("disabled");
                    }
                    else {
                        this.buttonDel.removeClass("disabled");
                    }
                    this.buttonAdd.removeClass("disabled");
                }
                else {
                    this.buttonAdd.addClass("disabled");
                    this.buttonDel.addClass("disabled");
                }
            };
            Editor.prototype.isSelected = function () {
                return !(this.selectedItem === null || this.selectedItem === undefined || this.selectedItem.length !== 1);
            };
            Editor.prototype.setupAddNewItemSelectOptions = function (curItem) {
                var strHtml = "<option value=\"\" disabled selected>Выберите тип</option>";
                if (curItem.Type === 0) {
                    strHtml += "<option value= \"1\">Справочник</option>";
                    strHtml += "<option value= \"2\">Данные</option>";
                }
                else if (curItem.Type === 1) {
                    strHtml += "<option value= \"3\">Справочник</option>";
                    strHtml += "<option value= \"1\">Группа</option>";
                }
                else if (curItem.Type === 2) {
                    strHtml += "<option value= \"4\">Данные</option>";
                    strHtml += "<option value= \"2\">Группа</option>";
                }
                this.typeSelect.html(strHtml);
                this.typeSelect.material_select();
            };
            Editor.prototype.AddNewItem = function () {
                if (this.isSelected()) {
                    var cirItemId = this.selectedItem.attr("id");
                    var curItem = this.ItemsTree[cirItemId];
                    this.setupAddNewItemSelectOptions(curItem);
                    this.dialogAdd.modal("open");
                }
            };
            Editor.prototype.DeleteItem = function () {
            };
            Editor.prototype.AddNewItemCompleted = function (That) {
                var errorMessage = "";
                var cirItemId = That.selectedItem.attr("id");
                var curItem = That.ItemsTree[cirItemId];
                var data = { command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-name").val(), Description: $("#editor-add-description").val(), Parent: curItem.Path };
                //let type: number = $("#editor-add-type").val();
                //let name: string = "" + $("#editor-add-name").val();
                if (data.Type === 0 || data.Type === null) {
                    errorMessage += (errorMessage === "" ? "" : "<br>") + "Не указан тип.";
                }
                if (data.Name === "") {
                    errorMessage += (errorMessage === "" ? "" : "<br>") + "Не задано имя.";
                }
                if (errorMessage !== "") {
                    dpas.app.showError(errorMessage);
                    return;
                }
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
var impEditor = View.Prj.Editor;
//# sourceMappingURL=editor.js.map