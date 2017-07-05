/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />
/// <reference path="editor-helpers.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var hljs;
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
                that.dataFields = that.NewClassData(null);
                $("#editor-tabs").tabs();
                that.tableFields = $("#table-fields");
                that.tableFieldsBody = $("#table-fields-body");
                that.typeSelect = $("#editor-add-field-type");
                that.typeSelect.material_select();
                that.codeViewTextarea = $("#editor-code-view-pre-code");
                that.dialogAdd = $("#editor-add-field").modal({ dismissible: false });
                that.btnDelete = $("#editor-designer-view-button-del");
                that.btnCopy = $("#editor-designer-view-button-copy");
                that.btnSave = $("#editor-designer-view-button-save");
                that.btnCancel = $("#editor-designer-view-button-cancel");
                //this.buttonDel.removeClass("disabled");
                that.SetupHandlers();
                that.Load();
            };
            EditorClass.prototype.NewClassData = function (newClassData) {
                if (newClassData === undefined || newClassData === null)
                    return { Inherited: "", Name: "", Path: "", Items: [] };
                for (var i = 0, icount = newClassData.Items.length; i < icount; i++) {
                    newClassData["field_" + newClassData.Items[i].Name] = newClassData.Items[i];
                }
                return newClassData;
            };
            EditorClass.prototype.setupField = function (field) {
                this.dataFields["field_" + field.Name] = field;
                this.dataFields.Items.push(field);
            };
            EditorClass.prototype.AddFieldData = function (newField) {
                var that = this;
                if (newField === undefined || newField === null) {
                    dpas.app.showError("Пустое поле!");
                    return false;
                }
                var errorMessage = "";
                if (newField.Type === undefined || newField.Type === null || newField.Type < 0 || newField.Type > 5 || (newField.Type === 5 && (newField.TypeClass === undefined || newField.TypeClass === null || newField.TypeClass === ""))) {
                    errorMessage += "Не указан Тип поля<br>";
                }
                if (newField.Name === undefined || newField.Name === null || newField.Name === "") {
                    errorMessage += "Не указано Имя поля<br>";
                }
                if (that.dataFields["field_" + newField.Name] !== undefined) {
                    errorMessage += "Указанное Имя поля уже используется<br>";
                }
                if (errorMessage !== "") {
                    dpas.app.showError(errorMessage);
                    return false;
                }
                //that.dataFields["field_" + newField.Name] = newField;
                //that.dataFields.Items.push(newField);
                var newRow = $(Prj.Helper.IFieldToHtml(newField, function (field) { that.setupField(field); }));
                newRow.click(function () {
                    that.SelectFieldRow($(this));
                });
                that.tableFieldsBody.append(newRow);
                that.SelectFieldRow(newRow);
                that.SetupViewSourceCode();
                return true;
            };
            EditorClass.prototype.SetupViewSourceCode = function () {
                this.codeViewTextarea.html(Prj.Helper.IClassToSourceCode(this.dataFields));
                $('pre code').each(function (i, block) {
                    hljs.highlightBlock(block);
                });
            };
            EditorClass.prototype.RemoveFieldData = function () {
                if (this.isSelected()) {
                    var field = this.GetSelectedField();
                    this.dataFields["field_" + field.Name] = undefined;
                    delete this.dataFields["field_" + field.Name];
                    this.dataFields.Items.splice(this.dataFields.Items.indexOf(field), 1);
                    this.rowSelected.remove();
                    this.SelectFieldRow(undefined);
                    this.btnSave.removeClass("disabled");
                    this.btnCancel.removeClass("disabled");
                }
                return true;
            };
            EditorClass.prototype.CopyFieldData = function () {
                if (this.isSelected()) {
                    var field = this.GetSelectedField();
                    $("#editor-add-field-type").val(field.Type);
                    $("#editor-add-field-type-name").val(field.TypeClass);
                    $("#editor-add-field-name").val(field.Name);
                    $("#editor-add-field-description").val(field.Description);
                    this.dialogAdd.modal("open");
                }
                return true;
            };
            EditorClass.prototype.isSelected = function () {
                return !(this.rowSelected === null || this.rowSelected === undefined || this.rowSelected.length !== 1);
            };
            EditorClass.prototype.GetSelectedFieldId = function () {
                if (this.isSelected())
                    return this.rowSelected.attr("id");
                return "";
            };
            EditorClass.prototype.GetSelectedField = function () {
                var cirItemId = this.GetSelectedFieldId();
                if (cirItemId !== "")
                    return this.dataFields["field_" + cirItemId];
                return undefined;
            };
            EditorClass.prototype.SelectFieldRow = function (row) {
                if (this.isSelected()) {
                    this.rowSelected.removeClass("dpas-tree-active");
                }
                this.rowSelected = row;
                if (this.isSelected()) {
                    this.rowSelected.addClass("dpas-tree-active");
                    this.btnCopy.removeClass("disabled");
                    this.btnDelete.removeClass("disabled");
                }
                else {
                    this.btnCopy.addClass("disabled");
                    this.btnDelete.addClass("disabled");
                }
            };
            EditorClass.prototype.SetupHandlers = function () {
                var that = this;
                $("#editor-add-field-form").submit(function (e) {
                    e.preventDefault();
                    //that.dialogAdd.modal("close");
                });
                $("#editor-add-field-apply").on("click", function () {
                    that.dialogAdd.modalResult = 0;
                    var newField = { Type: $("#editor-add-field-type").val(), TypeClass: $("#editor-add-field-type-name").val(), Name: $("#editor-add-field-name").val(), Description: $("#editor-add-field-description").val() };
                    //{ command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-field-type-name").val(), Description: $("#editor-add-field-description").val(), Parent: curItem.Path };
                    if (that.AddFieldData(newField)) {
                        that.btnSave.removeClass("disabled");
                        that.btnCancel.removeClass("disabled");
                        that.dialogAdd.modal("close");
                    }
                });
                that.btnDelete.on("click", function () { that.RemoveFieldData(); });
                that.btnCopy.on("click", function () { that.CopyFieldData(); });
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
                $("#editor-code-view-pre").height(h);
                $("#div-table-fields").height(h - $("#editor-designer-view-buttons").height() - $("#div-class-params").height() - 4);
                var w = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#editor-code-view-pre").width(w - 4);
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
                var that = this;
                var data = {
                    command: "readitem",
                    path: Prj.Editor.editor.GetSelectedItemPath().Path
                };
                dpas.app.postJson({
                    url: "/api/prj/editor", data: data,
                    success: function (result) {
                        if (result.result === true) {
                            that.LoadClassItem(that.NewClassData(result.item));
                        }
                        else {
                            dpas.app.showError(result.error);
                        }
                        dpas.app.hideLoading();
                    }
                });
            };
            EditorClass.prototype.LoadClassItem = function (data) {
                var that = this;
                var htmlString = "";
                that.dataFields = data;
                if (that.dataFields !== undefined && that.dataFields !== null) {
                    htmlString = Prj.Helper.IFieldsToHtml(that.dataFields.Items, function (field) { that.dataFields["field_" + field.Name] = field; });
                }
                that.SetupViewSourceCode();
                var rows = $(htmlString);
                rows.click(function () {
                    that.SelectFieldRow($(this));
                });
                that.tableFieldsBody.empty();
                that.tableFieldsBody.append(rows);
            };
            EditorClass.prototype.Save = function () {
                var that = this;
                var data = {
                    command: "saveitem",
                    path: Prj.Editor.editor.GetSelectedItemPath().Path,
                    data: that.dataFields
                };
                //let cirItemId: any = That.selectedItem.attr("id");
                //let curItem: any = That.ItemsTree[cirItemId];
                //let data: any = { command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-name").val(), Description: $("#editor-add-description").val(), Parent: curItem.Path };
                dpas.app.postJson({
                    url: "/api/prj/editor", data: data,
                    success: function (result) {
                        if (result.result === true) {
                            that.DisableButtons();
                        }
                        else {
                            dpas.app.showError(result.error);
                        }
                    }
                });
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