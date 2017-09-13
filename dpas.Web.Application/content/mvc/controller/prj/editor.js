/// <reference path="../../../ts/materialize.d.ts" />
/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var Editor = (function (_super) {
            __extends(Editor, _super);
            function Editor() {
                var _this = _super.call(this) || this;
                _this.ItemsTree = [];
                Editor.editor = _this;
                return _this;
            }
            Editor.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                var that = this;
                var content = $("#editor-content");
                dpas.app.navigateSetContent("/prj", content);
                $("#editor-menu-tree-view").treemenu({ delay: 300 });
                that.TreeProjectLoad(that);
                that.SelectApply(null);
                that.buttonAdd = $("#editor-menu-button-add");
                that.buttonAdd.on("click", function () {
                    that.AddNewItem();
                });
                that.buttonDel = $("#editor-menu-button-del");
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
                if (curItem.Type < 3) {
                    if (curItem.Items !== undefined) {
                        for (var i = 0, icount = curItem.Items.length; i < icount; i++) {
                            result += "<ul class=\"dpas-treemenu\">";
                            result += That.DrawItemTree(That, curItem.Items[i]);
                            result += "</ul>";
                        }
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
                        if (this.buttonDel !== undefined) {
                            this.buttonDel.addClass("disabled");
                        }
                    }
                    else {
                        if (this.buttonDel !== undefined) {
                            this.buttonDel.removeClass("disabled");
                        }
                    }
                    if (this.buttonAdd !== undefined) {
                        this.buttonAdd.removeClass("disabled");
                    }
                }
                else {
                    if (this.buttonAdd !== undefined) {
                        this.buttonAdd.addClass("disabled");
                    }
                    if (this.buttonDel !== undefined) {
                        this.buttonDel.addClass("disabled");
                    }
                }
            };
            Editor.prototype.GetSelectedItemId = function () {
                if (this.isSelected())
                    return this.selectedItem.attr("id");
                return "";
            };
            Editor.prototype.GetSelectedItem = function () {
                var cirItemId = this.GetSelectedItemId();
                if (!Prj.Helper.IsNullOrEmpty(cirItemId)) {
                    return this.ItemsTree[cirItemId];
                }
                return undefined;
            };
            Editor.prototype.GetSelectedItemPath = function () {
                var cirItem = this.GetSelectedItem();
                return (cirItem ? cirItem.Path : undefined);
            };
            Editor.prototype.UpdateSelectedItem = function (newPath, newName) {
                var cirItemId = this.GetSelectedItemId();
                this.selectedItem.html(this.DrawItemTree(this, this.GetSelectedItemPath()));
                this.ItemsTree[cirItemId] = newPath;
            };
            Editor.prototype.isSelected = function () {
                return !(this.selectedItem === null || this.selectedItem === undefined || this.selectedItem.length !== 1);
            };
            //private setupAddNewItemSelectOptions(curItem: any): void {
            //    let strHtml: string = "<option value=\"\" disabled selected>Выберите тип</option>";
            //    if (curItem.Type === 0) {
            //        strHtml += "<option value= \"1\">Справочник</option>";
            //        strHtml += "<option value= \"2\">Данные</option>";
            //    }
            //    else if (curItem.Type === 1) {
            //        strHtml += "<option value= \"3\">Справочник</option>";
            //        strHtml += "<option value= \"1\">Группа</option>";
            //    }
            //    else if (curItem.Type === 2) {
            //        strHtml += "<option value= \"4\">Данные</option>";
            //        strHtml += "<option value= \"2\">Группа</option>";
            //    }
            //    this.typeSelect.html(strHtml);
            //    this.typeSelect.material_select();
            //}
            Editor.prototype.viewDialogAdd = function (curItem) {
                var that = this;
                var strHtml = '<div class="modal">';
                strHtml += '    <div class="modal-content">';
                strHtml += '        <h4>Добавить</h4>';
                strHtml += '        <form id="editor-add-form" class="col s12">';
                strHtml += '            <div class="row" style="margin-bottom:0;">';
                strHtml += '                <div class="input-field col s12">';
                strHtml += '                    <select id="editor-add-type">';
                strHtml += "<option value=\"\" disabled selected>Выберите тип</option>";
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
                strHtml += '                    </select>';
                strHtml += '                    <label for="editor-add-name">Тип</label>';
                strHtml += '                </div>';
                strHtml += '            </div>';
                strHtml += '            <div class="row" style="margin-bottom:0;">';
                strHtml += '                <div class="input-field col s12">';
                strHtml += '                    <i class="material-icons prefix">playlist_add</i>';
                strHtml += '                    <input id="editor-add-name" name="addName" placeholder="Укажите имя" type="text" class="validate">';
                strHtml += '                    <label for="editor-add-name"></label>';
                strHtml += '                </div>';
                strHtml += '            </div>';
                strHtml += '            <div class="row" style="margin-bottom:0;">';
                strHtml += '                <div class="input-field col s12">';
                strHtml += '                    <i class="material-icons prefix">info_outline</i>';
                strHtml += '                    <input id="editor-add-description" name="addDescription"  placeholder="Укажите описание" type="text" class="validate">';
                strHtml += '                    <label for="editor-add-description"></label>';
                strHtml += '                </div>';
                strHtml += '            </div>';
                strHtml += '            <div class="modal-footer">';
                strHtml += '                <button class="btn-cancel modal-action waves-effect btn-flat btn-margin8">Отмена</button>';
                strHtml += '                <button class="btn-add modal-action waves-effect btn-flat btn-margin8" name="add">Создать</button>';
                strHtml += '            </div>';
                strHtml += '        </form>';
                strHtml += '    </div>';
                strHtml += '</div>';
                //private typeSelect: JQuery;
                //that.typeSelect = $("#editor-add-type");
                //that.typeSelect.material_select();
                var dialogAdd = $(strHtml).modal({
                    dismissible: false
                });
                dialogAdd.find("form").submit(function (e) {
                    e.preventDefault();
                    //dialogAdd.modal("close");
                    //that.AddNewItemCompleted(that);
                });
                var buttonCancel = dialogAdd.find(".btn-cancel");
                var buttonAdd = dialogAdd.find(".btn-add");
                var onClose = function (result) {
                    buttonCancel.off("click");
                    buttonAdd.off("click");
                    dialogAdd.modal('close');
                    if (result) {
                        var data = { command: "additem", Type: dialogAdd.find("#editor-add-type").val(), Name: dialogAdd.find("#editor-add-name").val(), Description: dialogAdd.find("#editor-add-description").val(), Parent: that.GetSelectedItemPath() };
                        that.AddNewItemCompleted(data);
                    }
                    dialogAdd.remove();
                };
                buttonCancel.on("click", function (event) {
                    onClose(false);
                });
                buttonAdd.on("click", function (event) {
                    onClose(true);
                });
                $("body").append(dialogAdd);
                dialogAdd.find("select").material_select();
                dialogAdd.modal("open");
            };
            Editor.prototype.AddNewItem = function () {
                if (this.isSelected()) {
                    var cirItemId = this.selectedItem.attr("id");
                    var curItem = this.ItemsTree[cirItemId];
                    this.viewDialogAdd(curItem);
                }
            };
            Editor.prototype.DeleteItem = function () {
            };
            Editor.prototype.AddNewItemCompleted = function (data) {
                var errorMessage = "";
                //let cirItemId: any = That.selectedItem.attr("id");
                //let curItem: any = That.ItemsTree[cirItemId];
                //let data: any = { command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-name").val(), Description: $("#editor-add-description").val(), Parent: That.GetSelectedItemPath() };
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