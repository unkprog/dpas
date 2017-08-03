/// <reference path="../../ts/jquery-3.1.1.d.ts" />
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
            $('#btnNewProject').on("click", function () {
                //$('#modal-prj-name').modal({
                //    dismissible: false,
                //    complete: function () { that.NewProject(); }
                //});
                that.modalNewProject.modal('open');
            });
            $("#btnOpenProject").on("click", function () {
                dpas.app.navigate({ url: "/nav/prj/list" });
            });
            //$('#btnTEst').on("click", function () {
            //    var data = { prjCode: "id" };
            //    dpas.app.postJson({
            //        url: '/api/prj/current', data: data,
            //        success: function (result:any):any {
            //            dpas.app.postJson({
            //                url: '/api/prj/editor',
            //                data: { command: "prjtree" },
            //                success: function (result) {
            //                    //showError(JSON.stringify(result.data));
            //                }
            //            });
            //        }
            //    });
            //});
        };
        Index.prototype.Dispose = function () {
            _super.prototype.Dispose.call(this);
        };
        return Index;
    }(dpas.Controller));
    View.Index = Index;
})(View = exports.View || (exports.View = {}));
new View.Index();
//# sourceMappingURL=index.js.map