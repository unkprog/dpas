declare var $, dpas: any;


export module View {
    export module Prj {
        export class EditorProject {
            Init() {
                var that = this;
                $('#btnPrjAdd').on("click", function () {
                    $('#modal-prj-name').modal({
                        dismissible: false,
                        //complete: function () { that.NewProject(); }
                    });

                    $('#modal-prj-name').modal('open');
                });
            }
        }
    }
}

(new View.Prj.EditorProject()).Init();