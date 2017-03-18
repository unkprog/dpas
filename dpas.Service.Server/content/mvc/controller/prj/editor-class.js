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
                that.tableFields = $("#table-fields");
                that.tableFieldsBody = $("#table-fields-body");
                that.typeSelect = $("#editor-add-field-type");
                that.typeSelect.material_select();
                that.dialogAdd = $("#editor-add-field").modal({ dismissible: false });
                that.btnDelete = $("#editor-designer-view-button-del");
                that.btnCopy = $("#editor-designer-view-button-copy");
                that.btnSave = $("#editor-designer-view-button-save");
                that.btnCancel = $("#editor-designer-view-button-cancel");
                //this.buttonDel.removeClass("disabled");
                that.SetupHandlers();
                that.Load();
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
                that.tableFieldsBody.append($(that.DrawClassField(newField)));
                return true;
            };
            EditorClass.prototype.DrawClassField = function (newField) {
                var result = "<tr id=\"";
                result += newField.Name;
                result += "\">";
                result += "<td>";
                result += newField.Name;
                result += "</td>";
                result += "<td>";
                result += this.GetTypeString(newField);
                result += "</td>";
                result += "<td>";
                result += newField.Description;
                result += "</td>";
                result += "</tr>";
                return result;
            };
            EditorClass.prototype.GetTypeString = function (field) {
                var type = "" + field.Type;
                if (type === "0")
                    return "Строка";
                if (type === "1")
                    return "Целое";
                if (type === "2")
                    return "Вещестенное";
                if (type === "3")
                    return "Логическое";
                if (type === "4")
                    return "Дата";
                return "Класс"; //field.TypeName;
            };
            EditorClass.prototype.SetupHandlers = function () {
                var that = this;
                $("#editor-add-field-form").submit(function (e) {
                    e.preventDefault();
                    //that.dialogAdd.modal("close");
                });
                $("#editor-add-field-apply").on("click", function () {
                    that.dialogAdd.modalResult = 0;
                    var newField = { Type: $("#editor-add-field-type").val(), TypeName: $("#editor-add-field-type-name").val(), Name: $("#editor-add-field-name").val(), Description: $("#editor-add-field-description").val() };
                    //{ command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-field-type-name").val(), Description: $("#editor-add-field-description").val(), Parent: curItem.Path };
                    if (that.AddFieldData(newField)) {
                        that.btnSave.removeClass("disabled");
                        that.btnCancel.removeClass("disabled");
                        that.dialogAdd.modal("close");
                    }
                });
                that.btnSave.on("click", function () { that.Save(); });
                that.btnCancel.on("click", function () { that.Cancel(); });
                $("#editor-add-field-cancel").on("click", function () {
                    that.dialogAdd.modalResult = 1;
                    that.dialogAdd.modal("close");
                });
                $("#editor-designer-view-button-add").on("click", function () {
                    that.dialogAdd.modal("open");
                });
            };
            EditorClass.prototype.ApplyLayout = function () {
                var h = $("#editor-content").height() - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);
                $("#div-table-fields").height(h - $("#editor-designer-view-buttons").height() - 4);
                var w = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);
            };
            EditorClass.prototype.Navigate = function (target) {
                impEditor.editor.Navigate(target);
            };
            EditorClass.prototype.DisableButtons = function () {
                this.btnCopy.addClass("disabled");
                this.btnDelete.addClass("disabled");
                this.btnSave.addClass("disabled");
                this.btnCancel.addClass("disabled");
            };
            EditorClass.prototype.Load = function () {
                dpas.app.showLoading();
                this.DisableButtons();
                dpas.app.hideLoading();
            };
            EditorClass.prototype.Save = function () {
                this.DisableButtons();
            };
            EditorClass.prototype.Cancel = function () {
                this.Load();
            };
            return EditorClass;
        }(dpas.Controller));
        Prj.EditorClass = EditorClass;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
var EditorClass = new View.Prj.EditorClass();
//# sourceMappingURL=editor-class.js.map