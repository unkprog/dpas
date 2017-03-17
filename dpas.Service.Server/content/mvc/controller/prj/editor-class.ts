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

            private dataFields: IClassData;

            public Initialize(): void {
                super.Initialize();

                
                let that: EditorClass = this;
                that.dataFields = that.NewClassData();

                $("#editor-tabs").tabs();
                that.typeSelect = $("#editor-add-field-type");
                that.typeSelect.material_select();

                that.dialogAdd = $("#editor-add-field").modal({ dismissible: false, complete: function (): void { that.AddNewItemCompleted(); } });
                that.SetupHandlers()
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

                return true;
            }

            private SetupHandlers() {
                let that: EditorClass = this;

                $("#editor-add-field-form").submit(function (e: JQueryEventObject): any {
                    e.preventDefault();
                    //that.dialogAdd.modal("close");
                });

                $("#editor-add-field-apply").on("click", function (): void {
                    that.dialogAdd.modalResult = 0;
                    if (that.AddFieldData({ Type: 0, TypeName: "", Name: "", Description: "" })) {
                        that.dialogAdd.modal("close");
                    }
                });

                $("#editor-add-field-cancel").on("click", function (): void {
                    that.dialogAdd.modalResult = 1;
                    that.dialogAdd.modal("close");
                });

                $("#editor-designer-view-button-add").on("click", function (): void {
                    that.dialogAdd.modal("open");
                });
            }

            private AddNewItemCompleted(): void {
                let that: EditorClass = this;
                if (that.dialogAdd.modalResult === 0) {
                    alert("Ok");
                }
                else if (that.dialogAdd.modalResult === 1) {
                    alert("Cancel");
                }
                else {
                    alert("Undefined");
                }
            }


            public ApplyLayout(): void {
                let h: number = $("#editor-content").height() - $("#editor-tabs").height();
                $("#editor-designer-view").height(h);
                $("#editor-code-view").height(h);
                $("#code-view-textarea").height(h);

                let w: number = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#code-view-textarea").width(w - 4);
            }


            public Navigate(target: Element): void {
                impEditor.editor.Navigate(target);
            }
        }
    }
}

let EditorClass: View.Prj.EditorClass = new View.Prj.EditorClass();