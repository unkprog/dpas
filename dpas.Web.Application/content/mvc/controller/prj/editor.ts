declare var $: any;
declare var navigate: any;
declare var treefilter: any;

export module View {
    export module Prj {
        export class Editor {

            public Init() {
                var that = this;
                $("#btnMainMenu").on("click", function (event) {
                    navigate("/nav/index");
                });

                $(".tree").treemenu({ delay: 300 }); //.openActive();
                $('ul.tabs').tabs();
                that.ApplyLayout();
                
                $(window).resize(function () {
                    that.ApplyLayout();
                });
                
            }

            ApplyLayout() {
                var h = window.innerHeight - $('.navbar-fixed').height();
                $('#editor-menu').height(h);
                $('#editor-menu-tree').height(h - 64);
                $('#editor-content').height(h);
                h = h - $('#editor-tabs').height();
                $('#editor-designer-view').height(h);
                $('#editor-code-view').height(h);
                $('#code-view-textarea').height(h);

                var w = window.innerWidth - $('#editor-menu').width();
                $('#editor-content').width(w);
                $('#code-view-textarea').width(w - 4);
            }
        }
    }
}


(new View.Prj.Editor()).Init();