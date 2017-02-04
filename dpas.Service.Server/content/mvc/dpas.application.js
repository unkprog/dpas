var exports = window.exports = window.exports || {};

(function () {
    var dpas = window.dpas = window.dpas || {};
    dpas.Application = function () {
        var that = this;
        function alertError(num) {
            //ошибки запроса
            var arr = ['Ваш браузер не поддерживает Ajax', 'Не удалось выполнить запрос', 'Адрес не существует', 'Время ожидания истекло']; //['Your browser does not support Ajax', 'Request failed', 'Address does not exist', 'The waiting time left'];
            alert(arr[num]);
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
                url: url,
                type: "GET",
                dataType: "html", cache: false, async: true, data: { ajax: true },
                success: function (html) {
                    if (callback)
                        callback(html);
                },
                error: alertError
            });
        };

        var loading = $('.dpas-loadbar');
        var content = $("#content");

        var currentUrl;
        that.navigateClear = function () {
            currentUrl = '';
        }

        that.showLoading = function () {
            loading.show();
        }
        that.hideLoading = function () {
            loading.hide();
        }
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

        that.callJson = function (options) {
            loading.show();
            var jsonData = options.url + " ---> " + options.data ? JSON.stringify(options.data) : undefined;
            console.log(jsonData);
            $.ajax({
                type: options.type, url: location.protocol + '//' + location.host + options.url, async: true, dataType: 'json', data: JSON.stringify(options.data), //contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (options.success)
                        options.success(result);
                    loading.hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    showError(thrownError);
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

        var navigateHistory = {
            count: 0,
            list: [],
            push: function (url) {
                list[count] = url;
                count++;
            },
            pop: function () {
                var url = list[count];
                count--;
                if (count < 0) count = 0;
                return url;
            },
        };

        that.navigate = function (url, options) {
            if (currentUrl !== url) {
                loading.show();
                // заносим ссылку в историю
                if (options && options.isPush === true && currentUrl && currentUrl !== "/nav/curpage")
                    navigateHistory.push(currentUrl);
                if (url !== "/nav/curpage")
                    currentUrl = url;

                content.css({ opacity: 0 });
                content.empty();
                dpas.app.loadHtml(url,
                           function (html) {

                               content.html(html);
                               //content.append($(html))
                               //var scripts = content[0].getElementsByTagName("script");
                               //for (var i = 0; i < scripts.length; ++i) {
                               //    var script = scripts[i];
                               //    eval(script.innerText);
                               //}
                               content.css({ opacity: 1 });
                               loading.hide();
                           });
            }
        }

    };
    dpas.app = new dpas.Application();
    return window.app;
})(window.dpas);