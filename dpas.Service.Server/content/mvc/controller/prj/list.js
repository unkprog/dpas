/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var List = (function (_super) {
            __extends(List, _super);
            function List() {
                _super.apply(this, arguments);
            }
            List.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                var that = this;
                this.LoadProjects(that);
                $('#btnRefresh').on("click", function () {
                    that.LoadProjects(that);
                });
            };
            List.prototype.LoadProjects = function (that) {
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
                                dpas.app.navigate({ url: "/nav/prj/editor" });
                            }
                        });
                    });
                }
            };
            return List;
        }(dpas.Controller));
        Prj.List = List;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
new View.Prj.List();
//# sourceMappingURL=list.js.map