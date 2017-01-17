﻿declare var $: any;
declare var navigate: any;
declare var dpas: any;

export module View {
    export module Prj {
        export class List {
            that: any;

            public LoadProjects(that: any) {
                dpas.app.postJson({
                    url: '/api/prj/list', data: {},
                    success: function (result) {
                        that.SetList(result);
                    }
                });
                //$.ajax({
                //    type: "POST", url: location.protocol + '//' + location.host + '/api/prj/list',
                //    dataType: "json",
                //    success: function (result) {
                //        that.SetList(result);
                //    },
                //    error: function (xhr, ajaxOptions, thrownError) {
                //        showError(thrownError);
                //    }
                //});
            }

            public Init() {

                this.that = this;
                this.LoadProjects(this.that);

                var th = this.that;
                $('#btnRefresh').on("click", function () {
                    th.LoadProjects(th);
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
                        navigate("/nav/prj/editor?prj=" + $(this).data('id'));
                    });
                }
            }
        }
    }
}

(new View.Prj.List()).Init();