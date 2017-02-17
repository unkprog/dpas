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

            public dialogAdd: JQuery;
            public Initialize(): void {
                super.Initialize();
               
                let that: Editor = this;
                this.dialogAdd = $("#editor-add").modal({ dismissible: false });
                let content: JQuery = $("#editor-content");
                dpas.app.navigateSetContent("/prj", content);

                $("#editor-menu-tree-view").treemenu({ delay: 300 });
                $("ul.tabs").tabs();
                that.TreeProjectLoad(that);
            }

            public ApplyLayout():void {
                let h: number = window.innerHeight - $(".navbar-fixed").height() - 22;
                $("#editor-menu").height(h);
                $("#editor-menu-tree").height(h - 80);
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

            private ItemsTree: any = [];
            public selectedItem: JQuery;
            private SetupTreeProject(That: Editor, dataTreeProject: any):void {
                That.ItemsTree = [];

                let drawItemTree: Function = function (curItem: any): string {

                    let result: string = "<li>";

                    result += "<a id=\"";
                    result += curItem.Path;
                    result += "\" class=\"ajax\" data-id=\"";
                    result += curItem.Code;
                    result += "\" href=\"/prj/editor-project";
                    result += curItem.Type === 0 ? "?project=" + curItem.Name : "/" + curItem.Path;
                    result += "\">";
                    That.ItemsTree.push(curItem);
                    That.ItemsTree[curItem.Code] = curItem;


                    result += curItem.Name;
                    result += "</a>";


                    if (curItem.Items !== undefined) {
                        for (let i: number = 0, icount: number = curItem.Items.length; i < icount; i++) {
                            result += "<ul>";
                            result += drawItemTree(curItem.Items[i]);
                            result += "</ul>";
                        }
                    }
                    result += "</li>";
                    return result;
                };

                let elsStr: string = "";
                let i: number = 0, icount: number = dataTreeProject.length;
                for (; i < icount; i++) {
                    elsStr += drawItemTree(dataTreeProject[i]);
                }

                $("#editor-menu-tree-view").html(elsStr).treemenu({ delay: 300 });
            }

            public AddNewItem(): void {
                if (this.selectedItem == null) {
                }
                this.dialogAdd.modal("open");
            }
        }
    }
}

new View.Prj.Editor();