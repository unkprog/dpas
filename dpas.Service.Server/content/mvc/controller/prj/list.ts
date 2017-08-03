/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor-helpers.ts" />

namespace View {
    export module Prj {

        export class List extends dpas.Controller {

            private modalNewProject: JQuery;
            private selectedProject: JQuery;
            public Initialize(): void {
                super.Initialize();

                let that = this;
                this.LoadProjects(that);

                $("#modal-prj-form").submit(function (e: JQueryEventObject): any {
                    e.preventDefault();
                    $("#modal-prj-name").modal("close");
                });


                that.modalNewProject = $("#modal-prj-name").modal({
                    dismissible: false,
                    complete: function (): void { that.NewProject(); }
                });

                $('#btnAddProject').on("click", function () {
                    that.modalNewProject.modal('open');
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
                        if (Helper.NotIsNull(that.selectedProject))
                            that.selectedProject.removeClass("active");
                        that.selectedProject = $(this);
                        that.selectedProject.addClass("active");

                        //var data = { prjCode: $(this).data('id') };
                        //dpas.app.postJson({
                        //    url: '/api/prj/current', data: data,
                        //    success: function (result) {
                        //        dpas.app.navigate({ url: "/nav/prj/editor" });
                        //    }
                        //});
                    });
                }
            }

            public NewProject(): void {
                //if ('' + $('#prjName').val() === '') {
                //    return;
                //}
                //dpas.app.navigateClear();
                //var data = { prjName: $('#prjName').val(), prjDescription: $('#prjDescription').val() };

                //dpas.app.postJson({
                //    url: '/api/prj/create', data: data,
                //    success: function (result) {
                //        if (result.result == true)
                //            dpas.app.navigate("/nav/prj/editor?prj=" + result.project.Code);
                //        else
                //            dpas.app.showError(result.error);
                //    }
                //});

                ////$.ajax({
                ////    type: "POST", url: location.protocol + '//' + location.host + '/api/prj/create',
                ////    async: true,
                ////    data: data,
                ////    dataType: "json",
                ////    success: function (result) {
                ////        if (result.resut == true)
                ////            navigate("/nav/prj/editor");
                ////        else
                ////            showError(result.error);
                ////    },
                ////    error: function (xhr, ajaxOptions, thrownError) {
                ////        showError(thrownError);
                ////    }
                ////});
            }
        }
    }
}

new View.Prj.List();