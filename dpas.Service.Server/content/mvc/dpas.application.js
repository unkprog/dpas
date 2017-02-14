var exports = window.exports = window.exports || {};

(function () {
    var dpas = window.dpas = window.dpas || {};
    dpas.Application = function () {
        var that = this;
        
        var loading;
        that.setLoadingElement = function (elLoading) {
            loading = elLoading;
        }

        that.showLoading = function () {
            if (loading)
                loading.show();
        }
        that.hideLoading = function () {
            if (loading)
                loading.hide();
        }

        var modal_error;
        var modal_error_content;
        that.showError = function (msg) {
            if (modal_error === undefined) {
                modal_error = $("#modal-error");
                modal_error_content = $("#modal-error-content");
                modal_error.modal({
                    dismissible: false,
                });
                $("#modal-error-ok").on("click", function () { modal_error.modal('close'); });
            }
            modal_error_content.html(msg);
            modal_error.modal('open');
        }

        function alertError(num) {
            //ошибки запроса
            var arr = ['Ваш браузер не поддерживает Ajax', 'Не удалось выполнить запрос', 'Адрес не существует', 'Время ожидания истекло']; //['Your browser does not support Ajax', 'Request failed', 'Address does not exist', 'The waiting time left'];
            that.showError(arr[num]);
        }


        that.loadScript = function (url, callback) {
            $.ajax({
                url: url,
                dataType: "script",
                success: function (script, textStatus, jqXHR) {
                    if (callback)
                        callback();
                },
                error: alertError
            });
        };

        that.loadHtml = function (url, callback) {
            $.ajax({
                url: url, type: "GET", dataType: "html", cache: false, async: true,
                success: function (html) {
                    if (callback)
                        callback(html);
                },
                error: alertError
            });
        };

        that.callJson = function (options) {
            if (options.showLoading === true)
                loading.show();
            var jsonData = options.url + " ---> " + options.data ? JSON.stringify(options.data) : undefined;
            console.log(jsonData);
            $.ajax({
                type: options.type, url: location.protocol + '//' + location.host + options.url, async: true, dataType: 'json', data: JSON.stringify(options.data), //contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (options.success)
                        options.success(result);
                    if (options.showLoading === true)
                        loading.hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.responseText)
                        that.showError(xhr.responseText);
                    else
                        that.showError(thrownError);
                    if (options.showLoading === true)
                        loading.hide();
                }
            });
        };

        that.postJson = function (options) {
            options.type = "POST";
            that.callJson(options);
        };


        that.getJson = function (options) {
            options.type = "GET";
            that.callJson(options);
        };

        that.getQueryParams = function (url) {
            var result = {};
            var sUrl = '' + url;
            var indexQs = sUrl ? sUrl.indexOf('?') : 0;
            if (indexQs > 0) {
                var params = sUrl.slice(indexQs + 1).split('&'), hash;
                for (var i = 0; i < params.length; i++) {
                    hash = params[i].split('=');
                    result[hash[0]] = hash[1];
                }
            }
            return result;
        };

        //var navigateHistory = {
        //    count: 0,
        //    list: [],
        //    push: function (url) {
        //        list[count] = url;
        //        count++;
        //    },
        //    pop: function () {
        //        var url = list[count];
        //        count--;
        //        if (count < 0) count = 0;
        //        return url;
        //    },
        //};
        var contents = {};

        that.navigateSetContent = function (prefix, content)
        {
            contents[prefix] = content;
        }

        var currentUrl;
        that.navigateClear = function () {
            currentUrl = '';
        }

        function navigateGetPrefix(ulr) {
            var prefix = '' + ulr;
            var idx = prefix.indexOf('/', prefix.charAt(0) === '/' ? 1 : 0);
            prefix = prefix.substring(0, idx);
            return prefix;
        }

        that.navigate = function (url, options) {
            that.showLoading();
            if (url === undefined || url === '') {
                that.showError("Не задана ссылка для перехода.");
                return;
            }

            if (currentUrl !== url) {
                loading.show();
                //// заносим ссылку в историю
                //if (options && options.isPush === true && currentUrl && currentUrl !== "/nav/curpage")
                //    navigateHistory.push(currentUrl);
                if (url !== "/nav/curpage")
                    currentUrl = url;
                
                var prefix = navigateGetPrefix(url);
                var content = contents[prefix];
                if (content === undefined) {
                    that.showError("Не определен элемент для содежимого.");
                    return;
                }

                content.css({ opacity: 0 });
                content.empty();
                dpas.app.getJson({
                    url: url, success: function (data) {
                        content.html(data.view);
                        //content.append($(html))
                        //var scripts = content[0].getElementsByTagName("script");
                        //for (var i = 0; i < scripts.length; ++i) {
                        //    var script = scripts[i];
                        //    eval(script.innerText);
                        //}
                        content.css({ opacity: 1 });

                        //loading.hide();
                        that.hideLoading();

                    }
                });
            }
        }

    };
    dpas.app = new dpas.Application();
    return window.app;
})(window.dpas);