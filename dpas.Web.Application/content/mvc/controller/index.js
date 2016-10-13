"use strict";
var View;
(function (View) {
    var Index = (function () {
        function Index() {
        }
        Index.prototype.ShotAuth = function () {
            // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
            $('#modal1').openModal();
        };
        return Index;
    }());
    View.Index = Index;
})(View = exports.View || (exports.View = {}));
function Init() {
    (new View.Index()).ShotAuth();
}
Init();
//# sourceMappingURL=index.js.map