"use strict";
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var Editor = (function () {
            function Editor() {
            }
            Editor.prototype.Init = function () {
                $("#btnMainMenu").on("click", function (event) {
                    navigate("/nav/index");
                });
                $("#editor-menu-tree-view").treemenu({ delay: 300 }); //.openActive();
                $('ul.tabs').tabs();
                $(window).resize(function () {
                    this.ApplyLayout();
                });
                this.ApplyLayout();
                this.TreeProjectLoad();
            };
            Editor.prototype.TreeProjectLoad = function () {
                var dataTreeProject = [{
                        'id': 1, 'name': 'Проект', 'path': 'Проект', 'type': 0,
                        'children': [{
                                'id': 2, 'name': 'Справочники', 'path': 'Проект/Справочники', 'type': 1,
                                'children': [{ 'id': 4, 'name': 'Базовый', 'path': 'Проект/Справочники/Базовый', 'type': 3 },
                                    { 'id': 5, 'name': 'ЕдИзм', 'path': 'Проект/Справочники/ЕдИзм', 'type': 3 }]
                            },
                            {
                                'id': 3, 'name': 'Данные', 'path': 'Проект/Данные', 'type': 2,
                                'children': [{ 'id': 6, 'name': 'Журнал1', 'path': 'Проект/Данные/Журнал1', 'type': 4 }, { 'id': 7, 'name': 'Журнал2', 'path': 'Проект/Данные/Журнал2', 'type': 4 }]
                            }]
                    }];
                this.SetupTreeProject(dataTreeProject);
            };
            Editor.prototype.SetupTreeProject = function (dataTreeProject) {
                var ids = [];
                var drawItemTree = function (curItem) {
                    var isReference = false || curItem.type === 0 || curItem.type === 3 || curItem.type === 4;
                    var result = '<li>';
                    //if (isReference) {
                    result += '<a id="';
                    result += curItem.path;
                    result += '" data-id="';
                    result += ids.length.toString();
                    result += '">';
                    ids.push(curItem);
                    //}
                    //else
                    //    result += '<span>';
                    result += curItem.name;
                    result += '</a>'; //isReference ? '</a>' : '</span>';
                    if (curItem.children !== undefined) {
                        for (var i = 0, icount = curItem.children.length; i < icount; i++) {
                            result += '<ul>';
                            result += drawItemTree(curItem.children[i]);
                            result += '</ul>';
                        }
                    }
                    result += '</li>';
                    return result;
                };
                var elsStr = '';
                var i = 0, icount = dataTreeProject.length;
                for (; i < icount; i++) {
                    elsStr += drawItemTree(dataTreeProject[i]);
                }
                $("#editor-menu-tree-view").html(elsStr).treemenu({ delay: 300 });
                var that = this;
                that['ids'] = ids;
                for (i = 0, icount = ids.length; i < icount; i++) {
                    var elItem = ids[i];
                    var el = $(document.getElementById(elItem.path));
                    //el.elItem = elItem;
                    el.click(function () {
                        alert(that['ids'][$(this).data('id')].path);
                    });
                }
            };
            Editor.prototype.ApplyLayout = function () {
                var h = window.innerHeight - $('.navbar-fixed').height() - 22;
                $('#editor-menu').height(h);
                $('#editor-menu-tree').height(h - 80);
                $('#editor-content').height(h);
                h = h - $('#editor-tabs').height();
                $('#editor-designer-view').height(h);
                $('#editor-code-view').height(h);
                $('#code-view-textarea').height(h);
                var w = window.innerWidth - $('#editor-menu').width() - 17;
                $('#editor-content').width(w);
                $('#code-view-textarea').width(w - 4);
            };
            return Editor;
        }());
        Prj.Editor = Editor;
    })(Prj = View.Prj || (View.Prj = {}));
})(View = exports.View || (exports.View = {}));
(new View.Prj.Editor()).Init();
//# sourceMappingURL=editor.js.map