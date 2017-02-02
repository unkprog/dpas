"use strict";
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var List = (function () {
            function List() {
            }
            List.prototype.LoadProjects = function (that) {
                dpas.app.postJson({
                    url: '/api/prj/list',
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
            };
            List.prototype.Init = function () {
                this.that = this;
                this.LoadProjects(this.that);
                var th = this.that;
                $('#btnRefresh').on("click", function () {
                    th.LoadProjects(th);
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
                                navigate("/nav/prj/editor");
                            }
                        });
                    });
                }
            };
            return List;
        }());
        Prj.List = List;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
(new View.Prj.List()).Init();
//# sourceMappingURL=list.js.map