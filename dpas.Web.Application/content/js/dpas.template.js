(function () {
    var dpas = window.dpas = window.dpas || {};
    dpas.Template = function () {

        var templatesDictionary = new Array();
        var bTrue = true;
        function parseTemplate(value) {
            var valueStr = (typeof value !== 'string') ? '' + value : value;
            var i = -1, len = valueStr.length, cur, prev, section = '';
            var result = 'var o= \'\';with(d){ ';

            while (i < len) {
                i++;
                prev = cur;
                cur = valueStr.charAt(i);
                if (cur === '\n') {
                    section += '\\n';
                }
                else if (cur === ':' && prev === '#') {
                    if (section !== '') { result += 'o+=\''; result += section.substring(0, section.length - 1); result += '\';'; section = ''; }
                    i++;
                    prev = cur;
                    cur = valueStr.charAt(i);
                    while (bTrue) { //cur !== '#') {
                        if (prev === ':' && cur === '#') {
                            result += section.substring(0, section.length - 1);
                            section = '';
                            break;
                        }
                        section += cur;
                        i++;
                        prev = cur;
                        cur = valueStr.charAt(i);
                    }
                    //if (prev !== ':') {
                    //    throw new Error("Parse error #: :#");
                    //}
                }
                else if (cur === '{') {
                    if (section !== '') { result += 'o+=\''; result += section; result += '\';'; section = ''; }
                    i++; cur = valueStr.charAt(i);
                    result += 'o+='; while (cur !== '}') { result += cur; i++; cur = valueStr.charAt(i); } result += ';';
                }
                else { section += cur; }
            }
            if (section !== '') { result += 'o+=\''; result += section; result += '\';'; }
            result += '} return o;';
            return result;
        }

        /**
         * @param {Object} options = { 
         *    id       : 'id'       - идентификатор для запоминания скомпилированного шаблона в кеше
         *    template : 'template' - строковое представление шаблона
         *    error    : err(msg)   - фунцкия обратного вызова обработки ошибки
         * }
         * @returns {func} Скомпилированный шаблон
         */
        this.getTemplate = function (options) {
            if (!options.template) { if (options.error) options.error("Не определено содержимое шаблона!"); else throw new Error("Не определено содержимое шаблона!"); }
            var temp;
            if (options.id)
                temp = templatesDictionary[options.id];
            if (!temp) {
                try {
                    var funcBody = parseTemplate(options.template);
                    temp = new Function('d', funcBody);
                    if (options.id)
                        templatesDictionary[options.id] = temp;
                }
                catch (e) {
                    if (options.error) options.error(e.message); else throw new Error(e.message);
                }
            }
            return temp;
        };
        /**
        * @param {Object} options = {
        *    id       : 'id'       - идентификатор закешированного шаблона
        *    template : 'template' - скомпилированный шаблон
        *    error    : err(msg)   - фунцкия обратного вызова обработки ошибки
        *}
        * @returns {Object} Результат выполнения шаблона
        */
        this.render = function (options) {
            var temp;
            if (options.id) temp = templatesDictionary[options.id];
            else temp = options.template;
            if (!temp) { if (options.error) options.error("Отсутствует скомпилированный шаблон!"); else throw new Error("Отсутствует скомпилированный шаблон!"); }

            var result = '';
            if (options.data.constructor === Array) {
                for (var i = 0, count = options.data.length; i < count; i++)
                    result += temp(options.data[i]);
                return result;
            }
            else result = temp(options.data);
            return result;
        };
    };

    dpas.template = new dpas.Template();
    return window.dpas;
})(window.dpas);