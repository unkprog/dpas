/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />

export namespace View {

    export module Prj {
        export class List extends dpas.Controller {

            public Initialize(): void {
                super.Initialize();

                let that = this;
                this.LoadProjects(that);

                $('#btnRefresh').on("click", function () {
                    that.LoadProjects(that);
                });
            }

            public LoadProjects(that: any) {
                dpas.app.postJson({
                    url: '/api/prj/list',
                    success: function (result) {
                        that.SetList(result);
                    }
                });
            }

            public SetList(data) {
                var ids = [];

                var elsStr = '', template, ids = [], templateContent = $('#list-projects-item-template').html();

                var i = 0, icount = data.length;
                for (; i < icount; i++) {
                    template = dpas.template.getTemplate({ id: 'list-projects-item-template', template: templateContent });
                    elsStr += dpas.template.render({ template: template, data: data[i] });
                    //elsStr += drawItem(data[i]);
                    ids[data[i].Code] = data[i];
                }

                $("#list-projects").html(elsStr);

                var that = this;
                that['ids'] = ids;
                for (i = 0; i < icount; i++) {
                    var el = $(document.getElementById(data[i].Code));
                    el.click(function () {
                        var data = { prjCode: $(this).data('id') };
                        dpas.app.postJson({
                            url: '/api/prj/current', data: data,
                            success: function (result) {
                                dpas.app.navigate("/nav/prj/editor");
                            }
                        });
                    });
                }
            }
        }
    }
}

new View.Prj.List();