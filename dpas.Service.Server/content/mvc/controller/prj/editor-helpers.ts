namespace View {
    export module Prj {
        export interface IField {
            Type: number;
            TypeClass: string;
            Name: string;
            Description: string;
        }

        export interface IClass {
            Inherited: string;
            IsAbstract: boolean;
            Name: string;
            Path: string;
            Type: number;
            Items: IField[];
        }

        type IFieldToHtmlCallback = (field: IField) => void;

        export class Helper {

            public static IsNull(obj: any): boolean {
                return (obj === undefined || obj === null);
            }

            public static NotIsNull(obj: any): boolean {
                return (obj !== undefined && obj !== null);
            }

            public static IsNullOrEmpty(obj: any): boolean {
                return (obj === undefined || obj === null || obj === "");
            }

            public static GetTypeString(field: IField): string {
                let type: string = "" + field.Type;
                if (type === "0") return "Строка";
                if (type === "1") return "Целое";
                if (type === "2") return "Вещестенное";
                if (type === "3") return "Логическое";
                if (type === "4") return "Дата";
                return "Класс"; //field.TypeName;
            }

            public static GetTypeSourceCode(field: IField): string {
                let type: string = "" + field.Type;
                if (type === "0") return "string";
                if (type === "1") return "int";
                if (type === "2") return "double";
                if (type === "3") return "bool";
                if (type === "4") return "DateTime";
                return field.TypeClass; //field.TypeName;
            }

            public static IFieldToHtml(field: IField, onToHtml: IFieldToHtmlCallback): string {
                let result: string = "<tr id=\"";
                result += field.Name; result += "\">";
                result += "<td>"; result += field.Name; result += "</td>";
                result += "<td>"; result += Helper.GetTypeString(field); result += "</td>";
                result += "<td>"; result += field.Description; result += "</td>";
                result += "</tr>";

                if (onToHtml)
                    onToHtml(field);

                return result;
            }

            public static IFieldsToHtml(fields: IField[], onToHtml: IFieldToHtmlCallback): string {

                let result: string = "";

                if (Helper.NotIsNull(fields)) {
                    let field: IField;
                    for (let i = 0, icount = fields.length; i < icount; i++) {
                        field = fields[i];
                        result += Helper.IFieldToHtml(field, onToHtml);
                    }
                }
                return result;
            }

            public static IClassToSourceCode(item: IClass): string {

                let result: string = "";

                if (Helper.NotIsNull(item)) {
                    let field: IField;
                    result += "namespace "; result += item.Path.replace(new RegExp("/", "g"), "."); item.Path; result += "<br>";
                    result += "{<br>";
                    result += "    public"; result += item.IsAbstract ? " abstract" : ""; result += " class "; result += "" + item.Name; result += Helper.IsNullOrEmpty(item.Inherited) ? "" : " : " + item.Inherited; result += "<br> ";
                    result += "    {<br>";
                    for (let i = 0, icount = item.Items.length; i < icount; i++) {
                        field = item.Items[i];
                        result += "        ";
                        result += Helper.GetTypeSourceCode(field);
                        result += " ";
                        result += field.Name;
                        result += " { get; set; }<br>";
                    }
                    result += "    }<br>";
                    result += "}<br>";
                }
                return result;
            }

        }
    }
}