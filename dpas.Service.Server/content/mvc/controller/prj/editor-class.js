/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var EditorClass = (function (_super) {
            __extends(EditorClass, _super);
            function EditorClass() {
                _super.apply(this, arguments);
            }
            EditorClass.prototype.Initialize = function () {
                _super.prototype.Initialize.call(this);
                var that = this;
                that.dataFields = that.NewClassData();
                $("#editor-tabs").tabs();
                that.typeSelect = $("#editor-add-field-type");
                that.typeSelect.material_select();
                that.dialogAdd = $("#editor-add-field").modal({ dismissible: false, complete: function () { that.AddNewItemCompleted(); } });
                that.SetupHandlers();
            };
            EditorClass.prototype.NewClassData = function () {
                return { Inherited: "", Items: [] };
            };
            EditorClass.prototype.AddFieldData = function (newField) {
                var that = this;
                if (newField === undefined || newField === null) {
                    dpas.app.showError("Пустое поле!");
                    return false;
                }
                var errorMessage = "";
                if (newField.Type === undefined || newField.Type === null || newField.Type < 0 || newField.Type > 5 || (newField.Type === 5 && (newField.TypeName === undefined || newField.TypeName === null || newField.TypeName === ""))) {
                    errorMessage += "Не указан Тип поля<br>";
                }
                if (newField.Name === undefined || newField.Name === null || newField.Name === "") {
                    errorMessage += "Не указано Имя поля<br>";
                }
                if (that.dataFields[newField.Name] !== undefined) {
                    errorMessage += "Указанное Имя поля уже используется<br>";
                }
                if (errorMessage !== "") {
                    dpas.app.showError(errorMessage);
                    return false;
                }
                that.dataFields[newField.Name] = newField;
                that.dataFields.Items.push(newField);
                return true;
            };
            EditorClass.prototype.SetupHandlers = function () {
                var that = this;
                $("#editor-add-field-form").submit(function (e) {
                    e.preventDefault();
                    //that.dialogAdd.modal("close");
                });
                $("#editor-add-field-apply").on("click", function () {
                    that.dialogAdd.modalResult = 0;
                    if (that.AddFieldData({ Type: 0, TypeName: "", Name: "", Description: "" })) {
                        that.dialogAdd.modal("close");
                    }
                });
                $("#editor-add-field-cancel").on("click", function () {
                    that.dialogAdd.modalResult = 1;
                    that.dialogAdd.modal("close");
                });
                $("#editor-designer-view-button-add").on("click", function () {
                    that.dialogAdd.modal("open");
                });
            };
            EditorClass.prototype.AddNewItemCompleted = function () {
                var that = this;
                if (that.dialogAdd.modalResult === 0) {
                    alert("Ok");
                }
                else if (that.dialogAdd.modalResult === 1) {
                    alert("Cancel");
                }
                else {
                    alert("Undefined");
                }
            };
            EditorClass.prototype.ApplyLayout = function () {
                var h = $("#editor-content").height() - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);
                var w = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);
            };
            EditorClass.prototype.Navigate = function (target) {
                impEditor.editor.Navigate(target);
            };
            return EditorClass;
        }(dpas.Controller));
        Prj.EditorClass = EditorClass;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var EditorClass = new View.Prj.EditorClass();
//# sourceMappingURL=editor-class.js.map