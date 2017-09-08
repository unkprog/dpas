/// <reference path="../../dpas.d.ts" />
/// <reference path="../../dpas.controller.ts" />
/// <reference path="editor.ts" />
/// <reference path="editor-helpers.ts" />
/// <reference path="../../../ts/materialize.d.ts" />



namespace View {
    export module Prj {

        export class EditorClass extends dpas.Controller {

            private dialogAdd: JQuery;
            private typeSelect: JQuery;

            private className: JQuery;
            private classParent: JQuery;
            private classAbstract: JQuery;

            private tableFields: JQuery;
            private tableFieldsBody: JQuery;
            private dataFields: IClass;

            private codeViewTextarea: JQuery;

            private rowSelected: JQuery;
            private btnDelete: JQuery;
            private btnCopy: JQuery;
            private btnSave: JQuery;
            private btnCancel: JQuery;

            public Initialize(): void {
                super.Initialize();

                
                let that: EditorClass = this;
                that.dataFields = that.NewClassData(null);

                $("#editor-tabs").tabs();
                that.className = $("#class_name");
                that.classParent = $("#class_parent");
                that.classAbstract = $("#class_abstract");
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
            }

            private NewClassData(newClassData: IClass): IClass {
                if (newClassData === undefined || newClassData === null)
                    return { Inherited: "", IsAbstract: false, Name: "", Path: "", Type:0, Items: [] };

                for (let i = 0, icount = newClassData.Items.length; i < icount; i++) {
                    newClassData["field_" + newClassData.Items[i].Name] = newClassData.Items[i];
                }
                return newClassData;
            }

            private setupField(field: IField): void {
                this.dataFields["field_" + field.Name] = field;
                this.dataFields.Items.push(field);
            }

            private AddFieldData(newField: IField): boolean {
                let that: EditorClass = this;

                if (newField === undefined || newField === null) {
                    dpas.app.showError("Пустое поле!");
                    return false;
                }

                let errorMessage: string = "";
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

                let newRow: JQuery = $(Helper.IFieldToHtml(newField, function (field: IField) { that.setupField(field); }));
                newRow.click(function () {
                    that.SelectFieldRow($(this));
                });

                that.tableFieldsBody.append(newRow);
                that.SelectFieldRow(newRow);
                that.SetupView();
                that.SetupViewSourceCode();
                

                return true;
            }

            private SetupView() {
                let that: EditorClass = this;
                that.className.val(that.dataFields.Name);
                if (!Helper.IsNullOrEmpty(that.dataFields.Name))
                    $("#label_class_name").addClass("active");
                that.classParent.val(that.dataFields.Inherited);
                if (!Helper.IsNullOrEmpty(that.dataFields.Inherited))
                    $("#label_class_parent").addClass("active");
                that.classAbstract.prop('checked', that.dataFields.IsAbstract);
            }

            private SetupViewSourceCode() {
                this.codeViewTextarea.html(Helper.IClassToSourceCode(this.dataFields));
               
                $('pre code').each(function (i, block) {
                    hljs.highlightBlock(block);
                });
            }

            private EnableSaveButtons() {
                this.btnSave.removeClass("disabled");
                this.btnCancel.removeClass("disabled");
            }

            private RemoveFieldData(): boolean {
                if (this.isSelected()) {
                    let field: IField = this.GetSelectedField();
                    this.dataFields["field_" + field.Name] = undefined;
                    delete this.dataFields["field_" + field.Name];
                    this.dataFields.Items.splice(this.dataFields.Items.indexOf(field), 1); 
                    this.rowSelected.remove();
                    this.SelectFieldRow(undefined);
                    this.EnableSaveButtons();
                }
                return true;
            }

            private CopyFieldData(): boolean {
                if (this.isSelected()) {
                    let field: IField = this.GetSelectedField();
                    $("#editor-add-field-type").val(field.Type);
                    $("#editor-add-field-type-name").val(field.TypeClass);
                    $("#editor-add-field-name").val(field.Name);
                    $("#editor-add-field-description").val(field.Description);
                    this.dialogAdd.modal("open");
                }
                return true;
            }

            private isSelected(): boolean {
                return !(this.rowSelected === null || this.rowSelected === undefined || this.rowSelected.length !== 1)
            }

            public GetSelectedFieldId(): string {
                if (this.isSelected())
                    return this.rowSelected.attr("id");
                return "";
            }

            public GetSelectedField(): IField {
                let cirItemId: string = this.GetSelectedFieldId();
                if (cirItemId !== "")
                    return this.dataFields["field_" + cirItemId];
                return undefined;
            }


            private SelectFieldRow(row: JQuery): void {

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
            }

            private SetupHandlers() {
                let that: EditorClass = this;

                //that.className.on('keydown', function (e) {
                //    if (e.which == 13)
                //        that.EnableSaveButtons();
                //});

                that.className.on('input', function () {
                    that.EnableSaveButtons();
                });

                that.classParent.on('input', function () {
                    that.EnableSaveButtons();
                });

                that.classAbstract.on('click', function () {
                    that.EnableSaveButtons();
                });

                $("#editor-add-field-form").submit(function (e: JQueryEventObject): any {
                    e.preventDefault();
                    //that.dialogAdd.modal("close");
                });

                $("#editor-add-field-apply").on("click", function (): void {
                    that.dialogAdd.modalResult = 0;
                    let newField: IField = { Type: $("#editor-add-field-type").val(), TypeClass: $("#editor-add-field-type-name").val(), Name: $("#editor-add-field-name").val(), Description: $("#editor-add-field-description").val() };
                    //{ command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-field-type-name").val(), Description: $("#editor-add-field-description").val(), Parent: curItem.Path };

                    if (that.AddFieldData(newField)) {
                        that.EnableSaveButtons();
                        that.dialogAdd.modal("close");
                    }
                });

                that.btnDelete.on("click", function (): void { that.RemoveFieldData(); });
                that.btnCopy.on("click", function (): void { that.CopyFieldData(); });

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
                $("#editor-code-view-pre").height(h);
                $("#div-table-fields").height(h - $("#editor-designer-view-buttons").height() - $("#div-class-params").height() - 4);

                let w: number = window.innerWidth - $("#editor-menu").width() - 17;
                $("#editor-content").width(w);
                $("#editor-code-view-pre").width(w - 4);

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

                let that: EditorClass = this;
                let data: any = {
                    command: "readitem",
                    path: Editor.editor.GetSelectedItemPath()
                };

                dpas.app.postJson({
                    url: "/api/prj/editor", data: data,
                    success: function (result: any): void {
                        if (result.result === true) {
                            that.LoadClassItem(that.NewClassData(result.item));
                          
                        } else {
                            dpas.app.showError(result.error);
                        }
                        dpas.app.hideLoading();
                    }
                });
            }

            private LoadClassItem(data: IClass): void {
                let that: EditorClass = this;
                let htmlString: string = "";

                that.dataFields = data;
                if (that.dataFields !== undefined && that.dataFields !== null) {
                    htmlString = Helper.IFieldsToHtml(that.dataFields.Items, (field: IField) => { that.dataFields["field_" + field.Name] = field });
                }
                that.SetupView();
                that.SetupViewSourceCode();

                let rows: JQuery = $(htmlString);
                rows.click(function () {
                    that.SelectFieldRow($(this));
                });
                that.tableFieldsBody.empty();
                that.tableFieldsBody.append(rows);
            }

            private Save(): void {
                let that: EditorClass = this;

                that.dataFields.Name = that.className.val();
                that.dataFields.Inherited = that.classParent.val();
                that.dataFields.IsAbstract = that.classAbstract.prop('checked');

                let data: any = {
                    command: "saveitem",
                    path: Editor.editor.GetSelectedItemPath(),
                    data: that.dataFields
                }

                //let cirItemId: any = That.selectedItem.attr("id");
                //let curItem: any = That.ItemsTree[cirItemId];
                //let data: any = { command: "additem", Type: $("#editor-add-type").val(), Name: $("#editor-add-name").val(), Description: $("#editor-add-description").val(), Parent: curItem.Path };


                dpas.app.postJson({
                    url: "/api/prj/editor", data: data,
                    success: function (result: any): void {
                        if (result.result === true) {
                            that.DisableButtons();
                            that.LoadClassItem(that.NewClassData(result.item));
                            Editor.editor.UpdateSelectedItem(that.dataFields.Path, that.dataFields.Name);
                        } else {
                            dpas.app.showError(result.error);
                        }
                    }
                });

                this.DisableButtons();
            }

            private Cancel(): void {
                this.Load();
            }
        }
    }
}

let EditorClass: View.Prj.EditorClass = new View.Prj.EditorClass();