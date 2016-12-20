declare var $: any;
declare var navigate: any;
declare var dpas: any;
declare var showError: any;

export module View {
    export module Prj {
        export class List {
            that: any;

            public LoadProjects(that:any) {
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

            public Init() {

                this.that = this;
                this.LoadProjects(this.that);

                var th = this.that;
                $('#btnRefresh').on("click", function () {
                    th.LoadProjects(th);
                });
            }

            

            public SetList(data) {
                var ids = [];

                //var drawItem = function (curItem) {
                //    var result = '';
                //    result += '<a id="';
                //    result += curItem.Name;
                //    result += '" data-id="';
                //    result += ids.length.toString();
                //    result += '" class="collection-item center">';
                //    result += curItem.Name;
                //    result += '</a>';
                  
                //    return result;
                //}

                var elsStr = '', template, ids = [], templateContent = $('#list-projects-item-template').html();

                var i = 0, icount = data.length;
                for (; i < icount; i++) {
                    template = dpas.template.getTemplate({ id: 'list-projects-item-template', template: templateContent });
                    elsStr += dpas.template.render({ template: template, data: data[i] });
                    //elsStr += drawItem(data[i]);
                    ids[data[i].Code] = data[i];
                }

                //var elsStr = '';
                //var i = 0, icount = data.length;
                //for (; i < icount; i++) {
                //    elsStr += drawItem(data[i]);
                //    ids.push(data[i]);
                //}
                $("#list-projects").html(elsStr);

                var that = this;
                that['ids'] = ids;
                for (i = 0; i < icount; i++) {
                    //var elItem = ids[i];
                    var el = $(document.getElementById(data[i].Code));
                    //el.elItem = elItem;
                    el.click(function () {
                        navigate("/nav/prj/editor?prj=" + $(this).data('id')); //that['ids'][$(this).data('id')].Code);
                    });
                }
            }
        }
    }
}


(new View.Prj.List()).Init();