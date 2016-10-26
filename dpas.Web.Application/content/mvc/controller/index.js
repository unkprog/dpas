"use strict";
var View;
(function (View) {
    var Index = (function () {
        function Index() {
        }
        Index.prototype.ShowCarousel = function () {
            // the "href" attribute of .modal-trigger must specify the modal ID that wants to be triggered
            // $('#modal1').openModal();
            $('.carousel').carousel({ no_wrap: true, padding: "10px" });
            $('.carousel').height(500);
        };
        return Index;
    }());
    View.Index = Index;
})(View = exports.View || (exports.View = {}));
function Init() {
    (new View.Index()).ShowCarousel();
}
Init();
//# sourceMappingURL=index.js.map