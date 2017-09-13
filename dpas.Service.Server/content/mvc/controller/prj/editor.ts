/// <reference path="../../../ts/materialize.d.ts" />
/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />

namespace View {
    export module Prj {
        export class Editor extends dpas.Controller {
           
            public static editor: Editor;

            constructor() {
                super();
                Editor.editor = this;
            }

          
            private ItemsTree: any = [];
            public selectedItem: JQuery;

            public Initialize(): void {
                super.Initialize();
               
                let that: Editor = this;

                let content: JQuery = $("#editor-content");
                dpas.app.navigateSetContent("/prj", content);

                $("#editor-menu-tree-view").treemenu({ delay: 300 });
                
                that.TreeProjectLoad(that);

                that.SelectApply(null);
                that.buttonAdd = $("#editor-menu-button-add");
                that.buttonAdd.on("click", function (): void {
                    that.AddNewItem();
                });

                that.buttonDel = $("#editor-menu-button-del");
                that.buttonDel.on("click", function (): void {
                    that.DeleteItem();
                });

            }

            public ApplyLayout():void {
                let h: number = window.innerHeight - $(".navbar-fixed").height() - 22;
                $("#editor-menu").height(h);
                $("#editor-menu-tree").height(h - 90 - $(".editor-menu-buttons").height());
                $("#editor-content").height(h);
            }
            
            private TreeProjectLoad(That: Editor): void {
                dpas.app.postJson({
                    url: "/api/prj/editor",
                    data: { command: "prjtree" },
                    success: function (result: any): any {
                        That.SetupTreeProject(That, result.data);
                    }
                });
            }


            private DrawItemTree(That: Editor, curItem: any): string {

                curItem.PathId = curItem.Path.replace(new RegExp("/", "g"), "_");

                let result: string = "<li";
                if (!(curItem.Items !== undefined && curItem.Items.length > 0) && !(curItem.Type === 1 || curItem.Type === 2)) {
                    result += " class=\"dpas-tree-empty\"";
                }

                result += "><span class=\"dpas-toggler\"></span>";
                //if (curItem.Type == 0) {
                //    result += "<i class=\"small material-icons left\" style=\"margin-right:5px\">create_new_folder</i>";
                //}
                result
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
                        for (let i: number = 0, icount: number = curItem.Items.length; i < icount; i++) {
                            result += "<ul class=\"dpas-treemenu\">";
                            result += That.DrawItemTree(That, curItem.Items[i]);
                            result += "</ul>";
                        }
                    }
                }
                result += "</li>";
                return result;
            }


            public SaveItemTree(curItem: any): void {
                this.ItemsTree.push(curItem);
                this.ItemsTree[curItem.PathId] = curItem;
            }

            public AppendItemTree(That: Editor, result: any): void {
                let itemHtml: string = That.DrawItemTree(That, result.data);
                if (result.data.Type === 1 || result.data.Type === 2)
                    itemHtml = "<ul class=\"dpas-treemenu\">" + itemHtml + "</ul>";
                That.selectedItem.parent().removeClass("dpas-tree-empty").addClass("dpas-tree-opened").addClass(".dpas-tree-active");
                That.selectedItem.parent().append(itemHtml);
                That.SelectApply($("#" + result.data.PathId));
            }

            private SetupTreeProject(That: Editor, dataTreeProject: any):void {
                That.ItemsTree = [];
                let elsStr: string = "";
                let i: number = 0, icount: number = dataTreeProject.length;
                for (; i < icount; i++) {
                    elsStr += That.DrawItemTree(That, dataTreeProject[i]);
                }

                $("#editor-menu-tree-view").html(elsStr).treemenu({ delay: 300 });
            }

            public Navigate(target: Element): void {
                Editor.editor.SelectApply($(target));
            }

            private buttonAdd: JQuery;
            private buttonDel: JQuery;
            public SelectApply(item: JQuery): void {
                if (this.isSelected()) {
                    this.selectedItem.removeClass("dpas-tree-active");
                }
                this.selectedItem = item;
                if (this.isSelected()) {
                    this.selectedItem.addClass("dpas-tree-active");
                    let itemData: any = this.ItemsTree[this.selectedItem.attr("id")];
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
            }

            public GetSelectedItemId(): string {
                if (this.isSelected())
                    return this.selectedItem.attr("id");
                return "";
            }

            public GetSelectedItem(): any {
                let cirItemId: string = this.GetSelectedItemId();
                if (!Helper.IsNullOrEmpty(cirItemId)) {
                    return this.ItemsTree[cirItemId];
                }
                return undefined;
            }

            public GetSelectedItemPath(): any {
                let cirItem: any = this.GetSelectedItem();
                return (cirItem ? cirItem.Path : undefined);
            }

            public UpdateSelectedItem(newPath: string, newName: string): void {
                let cirItemId: string = this.GetSelectedItemId();
                this.selectedItem.html(this.DrawItemTree(this, this.GetSelectedItemPath()));
                this.ItemsTree[cirItemId] = newPath;
            }

            private isSelected(): boolean {
                return !(this.selectedItem === null || this.selectedItem === undefined || this.selectedItem.length !== 1)
            }

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

            
            private viewDialogAdd(curItem: any) {
                let that = this;
                let strHtml: string = '<div class="modal">';
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
                let dialogAdd: JQuery = $(strHtml).modal({
                    dismissible: false
                });
               

                dialogAdd.find("form").submit(function (e: JQueryEventObject): any {
                    e.preventDefault();
                    //dialogAdd.modal("close");
                    //that.AddNewItemCompleted(that);
                });


                let buttonCancel = dialogAdd.find(".btn-cancel");
                let buttonAdd = dialogAdd.find(".btn-add");

                var onClose = function (result) {
                    buttonCancel.off("click");
                    buttonAdd.off("click");
                    dialogAdd.modal('close');
                    if (result) {
                        let data: any = { command: "additem", Type: dialogAdd.find("#editor-add-type").val(), Name: dialogAdd.find("#editor-add-name").val(), Description: dialogAdd.find("#editor-add-description").val(), Parent: that.GetSelectedItemPath() };

                        that.AddNewItemCompleted(data);
                    }
                    dialogAdd.remove();
                }

                buttonCancel.on("click", function (event) {
                    onClose(false);
                });
                buttonAdd.on("click", function (event) {
                    onClose(true);
                });

                $("body").append(dialogAdd);
                dialogAdd.find("select").material_select();
                dialogAdd.modal("open");

            }
            public AddNewItem(): void {
                if (this.isSelected()) {
                    let cirItemId: any = this.selectedItem.attr("id");
                    let curItem: any = this.ItemsTree[cirItemId];
                    this.viewDialogAdd(curItem);
                }
            }

            public DeleteItem(): void {
            }


            private AddNewItemCompleted(data:any): void {
                let errorMessage: string = "";
                //let cirItemId: any = That.selectedItem.attr("id");
                //let curItem: any = That.ItemsTree[cirItemId];
                //let data: any = { command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-name").val(), Description: $("#editor-add-description").val(), Parent: That.GetSelectedItemPath() };
                //let type: number = $("#editor-add-type").val();
                //let name: string = "" + $("#editor-add-name").val();
                if (data.Type === 0 || data.Type === null) {
                    errorMessage += (errorMessage === "" ? "" : "<br>")  + "Не указан тип.";
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
                    success: function (result: any): void {
                        if (result.result === true) {
                            Editor.editor.AppendItemTree(Editor.editor, result);
                        } else {
                            dpas.app.showError(result.error);
                        }
                    }
                });
            }
        }
    }
}

let editor: View.Prj.Editor = new View.Prj.Editor();
import impEditor = View.Prj.Editor;