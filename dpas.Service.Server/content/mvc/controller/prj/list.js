/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor-helpers.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var List = (function (_super) {
            __extends(List, _super);
            function List() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            List.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                var that = this;
                $("#modal-prj-form").submit(function (e) {
                    e.preventDefault();
                    $("#modal-prj-name").modal("close");
                });
                that.modalNewProject = $("#modal-prj-name").modal({
                    dismissible: false,
                    complete: function () { that.NewProject(); }
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
            };
            List.prototype.DisableButtons = function () {
                var that = this;
                that.listButtonDel.addClass("disabled");
            };
            List.prototype.EnableButtons = function () {
                var that = this;
                that.listButtonDel.removeClass("disabled");
            };
            List.prototype.LoadProjects = function () {
                var that = this;
                that.DisableButtons();
                dpas.app.postJson({
                    url: '/api/prj/list',
                    success: function (result) {
                        that.SetList(result);
                    }
                });
            };
            List.prototype.SetList = function (data) {
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
                        if (Prj.Helper.NotIsNull(that.selectedProject))
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
            };
            List.prototype.DelProject = function () {
                var that = this;
                var data = { prjCode: that.selectedProject.data('id') };
                dpas.app.showDialog({
                    msg: "Удалить проект <b>" + that.selectedProject.data('name') + "</b>?", isCancel: true,
                    callback: function (result) {
                        if (result.dialogResult) {
                            dpas.app.postJson({
                                url: '/api/prj/delete', data: data,
                                success: function (result) {
                                    that.LoadProjects();
                                }
                            });
                        }
                    }
                });
            };
            List.prototype.NewProject = function () {
                var that = this;
                var prjName = $('#prjName').val();
                if (Prj.Helper.IsNullOrEmpty(prjName)) {
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
            };
            List.prototype.OpenProject = function (prjCode) {
                var data = { prjCode: prjCode };
                dpas.app.postJson({
                    url: '/api/prj/current', data: data,
                    success: function (result) {
                        dpas.app.navigate({ url: "/nav/prj/editor" });
                    }
                });
            };
            return List;
        }(dpas.Controller));
        Prj.List = List;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var listPrj = new View.Prj.List();
//# sourceMappingURL=list.js.map