declare var $, dpas: any;

export module View {
    export module Prj {
        export class Editor {
            public SelectedItem: any;
            private ItemsTree: any = [];
            private That: any;

            constructor() {
                this.That = this;
            }
            
            Init() {

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
            }

            private ApplyLayout() {
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
            }

            private TreeProjectLoad(That: Editor) {
                var that = That;
                dpas.app.postJson({
                    url: '/api/prj/editor',
                    data: { command: "prjtree" },
                    success: function (result) {
                        that.SetupTreeProject(that, result.data);
                    }
                });
            }

            private SetupTreeProject(That: Editor, dataTreeProject: any) {
                That.ItemsTree = [];
                let id = 0;
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
                }

                var elsStr = '';
                var i = 0, icount = dataTreeProject.length;
                for (; i < icount; i++) {
                    elsStr += drawItemTree(dataTreeProject[i]);
                }

                $("#editor-menu-tree-view").html(elsStr).treemenu({ delay: 300 });
            }
        }
    }
}

export let View_Prj_Editor: View.Prj.Editor = (new View.Prj.Editor()).Init();