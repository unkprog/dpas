/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />

namespace View {
    export module Prj {
        interface IFieldData {
            Type: number;
            TypeName: string;
            Name: string;
            Description: string;
        }

        interface IClassData {
            Inherited: string;
            Items: IFieldData[];
        }

        export class EditorClass extends dpas.Controller {

       

            private dialogAdd: JQuery;
            private typeSelect: JQuery;

            private tableFields: JQuery;
            private tableFieldsBody: JQuery;
            private dataFields: IClassData;

            private rowSelected: JQuery;
            private btnDelete: JQuery;
            private btnCopy: JQuery;
            private btnSave: JQuery;
            private btnCancel: JQuery;

            public Initialize(): void {
                super.Initialize();

                
                let that: EditorClass = this;
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
            }

            private NewClassData() {
                return { Inherited: "", Items: [] };
            }

            private AddFieldData(newField: IFieldData): boolean {
                let that: EditorClass = this;

                if (newField === undefined || newField === null) {
                    dpas.app.showError("Пустое поле!");
                    return false;
                }

                let errorMessage: string = "";
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
            }


            private DrawClassField(newField: IFieldData): string {
                let result: string = "<tr id=\"";
                result += newField.Name; result += "\">";
                result += "<td>"; result += newField.Name; result += "</td>";
                result += "<td>"; result += this.GetTypeString(newField); result += "</td>";
                result += "<td>"; result += newField.Description; result += "</td>";
                result += "</tr>";
                return result;
            }


            private GetTypeString(field: IFieldData) {
                let type: string = "" + field.Type;
                if (type === "0") return "Строка";
                if (type === "1") return "Целое";
                if (type === "2") return "Вещестенное";
                if (type === "3") return "Логическое";
                if (type === "4") return "Дата";
                return "Класс"; //field.TypeName;
            }

            private SetupHandlers() {
                let that: EditorClass = this;

                $("#editor-add-field-form").submit(function (e: JQueryEventObject): any {
                    e.preventDefault();
                    //that.dialogAdd.modal("close");
                });

                $("#editor-add-field-apply").on("click", function (): void {
                    that.dialogAdd.modalResult = 0;
                    let newField: IFieldData = { Type: $("#editor-add-field-type").val(), TypeName: $("#editor-add-field-type-name").val(), Name: $("#editor-add-field-name").val(), Description: $("#editor-add-field-description").val() };
                    //{ command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-field-type-name").val(), Description: $("#editor-add-field-description").val(), Parent: curItem.Path };

                    if (that.AddFieldData(newField)) {
                        that.btnSave.removeClass("disabled");
                        that.btnCancel.removeClass("disabled");
                        that.dialogAdd.modal("close");
                    }
                });

                that.btnSave.on("click", function (): void { that.Save(); });
                that.btnCancel.on("click", function (): void { that.Cancel(); });


                $("#editor-add-field-cancel").on("click", function (): void {
                    that.dialogAdd.modalResult = 1;
                    that.dialogAdd.modal("close");
                });

                $("#editor-designer-view-button-add").on("click", function (): void {
                    that.dialogAdd.modal("open");
                });
            }

            public ApplyLayout(): void {
                let h: number = $("#editor-content").height() - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);
                $("#div-table-fields").height(h - $("#editor-designer-view-buttons").height() - 4);

                let w: number = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);

            }


            public Navigate(target: Element): void {
                impEditor.editor.Navigate(target);
            }

            private DisableButtons() {
                this.btnCopy.addClass("disabled");
                this.btnDelete.addClass("disabled");
                this.btnSave.addClass("disabled");
                this.btnCancel.addClass("disabled");
            }
            private Load(): void {
                dpas.app.showLoading();
                this.DisableButtons();
                dpas.app.hideLoading();
            }

            private Save(): void {
                this.DisableButtons();
            }

            private Cancel(): void {
                this.Load();
            }
        }
    }
}

let EditorClass: View.Prj.EditorClass = new View.Prj.EditorClass();