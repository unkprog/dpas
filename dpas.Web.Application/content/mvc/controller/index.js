"use strict";
/// <reference path="../../ts/jquery-3.1.1.d.ts" />
/// <reference path="../dpas.d.ts" />
/// <reference path="../dpas.controller.ts" />
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
Object.defineProperty(exports, "__esModule", { value: true });
var View;
(function (View) {
    var Index = (function (_super) {
        __extends(Index, _super);
        function Index() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        Index.prototype.Initialize = function () {
            _super.prototype.Initialize.call(this);
            var that = this;
            $('#btnNewProject').on("click", function () {
                that.modalNewProject.modal('open');
            });
            $("#btnOpenProject").on("click", function () {
                dpas.app.navigate({ url: "/nav/prj/list" });
            });
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