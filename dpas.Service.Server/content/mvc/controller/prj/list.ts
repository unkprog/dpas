/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor-helpers.ts" />

namespace View {
    export module Prj {

        export class List extends dpas.Controller {

            private modalNewProject: JQuery;
            private selectedProject: JQuery;
            private listButtonAdd: JQuery;
            private listButtonDel: JQuery;

            public Initialize(): void {
                super.Initialize();
                let that = this;

                $("#modal-prj-form").submit(function (e: JQueryEventObject): any {
                    e.preventDefault();
                    $("#modal-prj-name").modal("close");
                });

                that.modalNewProject = $("#modal-prj-name").modal({
                    dismissible: false,
                    complete: function (): void { that.NewProject(); }
                });

                that.listButtonAdd = $('#list-button-add');
                that.listButtonAdd.on("click", function () {
                    that.modalNewProject.modal('open');
                });

                that.listButtonDel = $('#list-button-del');
                that.listButtonDel.on("click", function () {
                    that.DelProject();
                });

                that.LoadProjects();
            }

            private DisableButtons() {
                let that = this;
                that.listButtonDel.addClass("disabled");
            }

            private EnableButtons() {
                let that = this;
                that.listButtonDel.removeClass("disabled");
            }

            public LoadProjects() {
                let that = this;
                that.DisableButtons();
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
                        that.EnableButtons();
                    });
                    el = $(document.getElementById("open-" + data[i].Code));
                    el.click(function () {
                        that.OpenProject($(this).data('id'));
                    });
                }
            }

            public DelProject(): void {
                var that = this;
                var data = { prjCode: that.selectedProject.data('id') };
                dpas.app.showDialog(<dpas.IDialogOptions>{
                    msg: "Удалить проект <b>" + that.selectedProject.data('name') + "</b>?", isCancel: true,
                    callback: function (result: any) {
                        if (result.dialogResult) {
                            dpas.app.postJson({
                                url: '/api/prj/delete', data: data,
                                success: function (result) {
                                    that.LoadProjects();
                                }
                            });
                        }
                    }});
                
            }

            public NewProject(): void {
                let that = this;
                let prjName: string = $('#prjName').val();
                if (Helper.IsNullOrEmpty(prjName)) {
                    dpas.app.showError("Не указано имя проекта.");
                    return;
                }

                var data = { prjName: prjName, prjDescription: $('#prjDescription').val() };
                dpas.app.postJson({
                    url: '/api/prj/create', data: data,
                    success: function (result) {
                        if (result.result == true)
                            that.OpenProject(result.project.Code);
                        else
                            dpas.app.showError(result.error);
                    }
                });
            }

            private OpenProject(prjCode: string): void {
                var data = { prjCode: prjCode };
                dpas.app.postJson({
                    url: '/api/prj/current', data: data,
                    success: function (result) {
                        dpas.app.navigate({ url: "/nav/prj/editor" });
                    }
                });
            }
        }
    }
}

let listPrj: View.Prj.List = new View.Prj.List();
