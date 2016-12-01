declare var $: any;
declare var navigate: any;

export module View {
    export module Prj {
        export class Editor {
            public Init() {
                var that = this;
                $("#btnMainMenu").on("click", function (event) {
                    navigate("/nav/index");
                });
            }
        }
    }
}


(new View.Prj.Editor()).Init();