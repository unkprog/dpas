"use strict";
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var List = (function () {
            function List() {
            }
            List.prototype.Init = function () {
                var that = this;
                $.ajax({
                    type: "POST", url: location.protocol + '//' + location.host + '/api/prj/list',
                    async: true,
                    dataType: "json",
                    success: function (result) {
                        that.SetList(result);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        showError(thrownError);
                    }
                });
            };
            List.prototype.SetList = function (data) {
                var ids = [];
                var drawItem = function (curItem) {
                    var result = '';
                    result += '<a id="';
                    result += curItem.Name;
                    result += '" data-id="';
                    result += ids.length.toString();
                    result += '" class="collection-item center">';
                    result += curItem.Name;
                    result += '</a>';
                    return result;
                };
                var ids = [];
                var elsStr = '';
                var i = 0, icount = data.length;
                for (; i < icount; i++) {
                    elsStr += drawItem(data[i]);
                    ids.push(data[i]);
                }
                $("#list-projects").html(elsStr);
                var that = this;
                that['ids'] = ids;
                for (i = 0, icount = ids.length; i < icount; i++) {
                    var elItem = ids[i];
                    var el = $(document.getElementById(elItem.Name));
                    //el.elItem = elItem;
                    el.click(function () {
                        alert(that['ids'][$(this).data('id')].Name);
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