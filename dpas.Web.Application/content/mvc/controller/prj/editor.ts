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
            }
        }
    }
}


(new View.Prj.Editor()).Init();