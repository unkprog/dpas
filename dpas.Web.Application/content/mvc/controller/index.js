/// <reference path="../dpas.d.ts" />
/// <reference path="../dpas.controller.ts" />
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var View;
(function (View) {
    var Index = (function (_super) {
        __extends(Index, _super);
        function Index() {
            _super.apply(this, arguments);
        }
        Index.prototype.Initialize = function () {
            _super.prototype.Initialize.call(this);
            var that = this;
            $("#modal-prj-form").submit(function (e) {
                e.preventDefault();
                $('#modal-prj-name').modal('close');
            });
            $('#modal-prj-name').modal({
                dismissible: false,
                complete: function () { that.NewProject(); }
            });
            $('#btnNewProject').on("click", function () {
                $('#modal-prj-name').modal({
                    dismissible: false,
                    complete: function () { that.NewProject(); }
                });
                $('#modal-prj-name').modal('open');
            });
            $('#btnOpenProject').on("click", function () {
                dpas.app.navigate({ url: "/nav/prj/list" });
            });
            $('#btnTEst').on("click", function () {
                var data = { prjCode: "id" };
                dpas.app.postJson({
                    url: '/api/prj/current', data: data,
                    success: function (result) {
                        dpas.app.postJson({
                            url: '/api/prj/editor',
                            data: { command: "prjtree" },
                            success: function (result) {
                                //showError(JSON.stringify(result.data));
                            }
                        });
                    }
                });
            });
        };
        Index.prototype.Dispose = function () {
            _super.prototype.Dispose.call(this);
        };
        Index.prototype.NewProject = function () {
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
        };
        return Index;
    }(dpas.Controller));
    View.Index = Index;
})(View = exports.View || (exports.View = {}));
new View.Index();
//# sourceMappingURL=mvc/controller/index.js.map
//# sourceMappingURL=index.js.map