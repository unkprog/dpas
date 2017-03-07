/// <reference path="../../../ts/materialize.d.ts" />
/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="../../dpas.d.ts" />


namespace View {
    export module Prj {
        export class Editor extends dpas.Controller {
           
            public static editor: Editor;

            constructor() {
                super();
                Editor.editor = this;
            }

            private dialogAdd: JQuery;
            private typeSelect: JQuery;
            private ItemsTree: any = [];
            public selectedItem: JQuery;

            public Initialize(): void {
                super.Initialize();
               
                let that: Editor = this;

                that.typeSelect = $("select").material_select();
                that.dialogAdd = $("#editor-add").modal({ dismissible: false, complete: function (): void { that.AddNewItemCompleted(that); } });

                let content: JQuery = $("#editor-content");
                dpas.app.navigateSetContent("/prj", content);

                $("#editor-menu-tree-view").treemenu({ delay: 300 });
                
                that.TreeProjectLoad(that);

                $("#editor-add-form").submit(function (e: JQueryEventObject): any {
                    e.preventDefault();
                    that.dialogAdd.modal("close");
                });

                that.buttonAdd = $("#editor-menu-button-add");
                that.buttonDel = $("#editor-menu-button-del");
                that.SelectApply(null);
                that.buttonAdd.on("click", function (): void {
                    that.AddNewItem();
                });

                that.buttonDel.on("click", function (): void {
                    that.DeleteItem();
                });

            }

            public ApplyLayout():void {
                let h: number = window.innerHeight - $(".navbar-fixed").height() - 22;
                $("#editor-menu").height(h);
                $("#editor-menu-tree").height(h - 90 - $(".editor-menu-buttons").height());
                $("#editor-content").height(h);
                h = h - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);

                let w: number = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);
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

                let result: string = "<li";
                if (!(curItem.Items !== undefined && curItem.Items.length > 0) && !(curItem.Type === 1 || curItem.Type === 2)) {
                    result += " class=\"dpas-tree-empty\"";
                }

                result += "><span class=\"dpas-toggler\"></span>";

                result += "<a id=\"";
                result += curItem.Path;
                result += "\" class=\"ajax\" href=\"/prj/editor-project";
                result += curItem.Type === 0 ? "?project=" + curItem.Name : "?projectitem=" + curItem.Path.replace("/", "_");
                result += "\">";
                That.SaveItemTree(curItem);

                result += curItem.Name;
                result += "</a>";

                if (curItem.Items !== undefined) {
                    for (let i: number = 0, icount: number = curItem.Items.length; i < icount; i++) {
                        result += "<ul class=\"dpas-treemenu\">";
                        result += That.DrawItemTree(That, curItem.Items[i]);
                        result += "</ul>";
                    }
                }
                result += "</li>";
                return result;
            }

            public SaveItemTree(curItem: any): void {
                this.ItemsTree.push(curItem);
                this.ItemsTree[curItem.Path] = curItem;
            }

            public AppendItemTree(That: Editor, result: any): void {
                let itemHtml: string = That.DrawItemTree(That, result.data);
                if (result.data.Type === 1 || result.data.Type === 2)
                    itemHtml = "<ul class=\"dpas-treemenu\">" + itemHtml + "</ul>";
                That.selectedItem.parent().removeClass("dpas-tree-empty").addClass("dpas-tree-opened").addClass(".dpas-tree-active");
                That.selectedItem.parent().append(itemHtml);
                That.SaveItemTree(result.data);
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
                if (this.selectedItem != null) {
                    this.selectedItem.removeClass("dpas-tree-active");
                }
                this.selectedItem = item;
                if (this.selectedItem != null) {
                    this.selectedItem.addClass("dpas-tree-active");
                    let itemData: any = this.ItemsTree[this.selectedItem.attr("id")];
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
            }

            public AddNewItem(): void {
                if (this.selectedItem == null) {
                    return;
                }
                this.dialogAdd.modal("open");
            }

            public DeleteItem(): void {
            }


            private AddNewItemCompleted(That: Editor): void {
                let errorMessage: string = "";
                let data: any = { command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-name").val(), Description: $("#editor-add-description").val(), Parent: That.selectedItem.attr("id") };
                //let type: number = $("#editor-add-type").val();
                //let name: string = "" + $("#editor-add-name").val();
                if (data.Type === 0) {
                    errorMessage += (errorMessage === "" ? "" : "\n")  + "Не указан тип.";
                }
                if (data.Name === "") {
                    errorMessage += (errorMessage === "" ? "" : "\n") + "Не задано имя.";
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