declare var $: any;
//declare var navigate: any;
//declare var treefilter: any;
declare var showError: any;

export module View {
    export module Prj {
        export class List {
            public Init() {
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
            }


            public SetList(data) {
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
                }

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
            }
        }
    }
}


(new View.Prj.List()).Init();