"use strict";
var View;
(function (View) {
    var Index = (function () {
        function Index() {
        }
        Index.prototype.Init = function () {
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
        };
        Index.prototype.NewProject = function () {
            if ('' + $('#prjName').val() === '') {
                return;
            }
            navigateClear();
            var data = { prjName: $('#prjName').val(), prjComment: $('#prjComment').val() };
            $.ajax({
                type: "POST", url: location.protocol + '//' + location.host + '/api/prj',
                async: true,
                data: data,
                dataType: "json",
                success: function (result) {
                    navigate("/nav/prj/editor");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    showError(thrownError);
                }
            });
        };
        return Index;
    }());
    View.Index = Index;
})(View = exports.View || (exports.View = {}));
(new View.Index()).Init();
//# sourceMappingURL=index.js.map