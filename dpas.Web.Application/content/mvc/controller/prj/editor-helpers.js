var View;
(function (View) {
    var Prj;
    (function (Prj) {
        var Helper = (function () {
            function Helper() {
            }
            Helper.IsNull = function (obj) {
                return (obj === undefined || obj === null);
            };
            Helper.NotIsNull = function (obj) {
                return (obj !== undefined && obj !== null);
            };
            Helper.IsNullOrEmpty = function (obj) {
                return (obj === undefined || obj === null || obj === "");
            };
            Helper.GetTypeString = function (field) {
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
            Helper.GetTypeSourceCode = function (field) {
                var type = "" + field.Type;
                if (type === "0")
                    return "string";
                if (type === "1")
                    return "int";
                if (type === "2")
                    return "double";
                if (type === "3")
                    return "bool";
                if (type === "4")
                    return "DateTime";
                return field.TypeClass; //field.TypeName;
            };
            Helper.IFieldToHtml = function (field, onToHtml) {
                var result = "<tr id=\"";
                result += field.Name;
                result += "\">";
                result += "<td>";
                result += field.Name;
                result += "</td>";
                result += "<td>";
                result += Helper.GetTypeString(field);
                result += "</td>";
                result += "<td>";
                result += field.Description;
                result += "</td>";
                result += "</tr>";
                if (onToHtml)
                    onToHtml(field);
                return result;
            };
            Helper.IFieldsToHtml = function (fields, onToHtml) {
                var result = "";
                if (Helper.NotIsNull(fields)) {
                    var field = void 0;
                    for (var i = 0, icount = fields.length; i < icount; i++) {
                        field = fields[i];
                        result += Helper.IFieldToHtml(field, onToHtml);
                    }
                }
                return result;
            };
            Helper.IClassToSourceCode = function (item) {
                var result = "";
                if (Helper.NotIsNull(item)) {
                    var field = void 0;
                    result += "namespace ";
                    result += item.Path.replace(new RegExp("/", "g"), ".");
                    item.Path;
                    result += "<br>";
                    result += "{<br>";
                    result += "    public class ";
                    result += "" + item.Name;
                    result += Helper.IsNullOrEmpty(item.Inherited) ? "" : " : " + item.Inherited;
                    result += "<br>";
                    result += "    {<br>";
                    for (var i = 0, icount = item.Items.length; i < icount; i++) {
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
            };
            return Helper;
        }());
        Prj.Helper = Helper;
    })(Prj = View.Prj || (View.Prj = {}));
})(View || (View = {}));
//# sourceMappingURL=editor-helpers.js.map