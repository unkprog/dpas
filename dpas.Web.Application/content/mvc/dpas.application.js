//var require = function () {

//};

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
                that.showLoading();
            var jsonData = options.url + " ---> " + options.data ? JSON.stringify(options.data) : undefined;
            console.log(jsonData);
            $.ajax({
                type: options.type, url: location.protocol + '//' + location.host + options.url, async: true, dataType: 'json', data: JSON.stringify(options.data), //contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (options.success)
                        options.success(result);
                    if (options.showLoading === true)
                        that.hideLoading(); 
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (xhr.responseText)
                        that.showError(xhr.responseText);
                    else
                        that.showError(thrownError);
                    if (options.showLoading === true)
                        that.hideLoading();
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
        var navigateData = {
            url: '',
            contents: {},
            controllers: { notifyEvent: [] }
        };

        that.navigateSetContent = function (prefix, content) {
            navigateData.contents[prefix] = content;
        }

        var currentUrl;
        that.navigateClear = function () {
            navigateData.url = '';
        }

        function navigateGetPrefix(ulr) {
            var prefix = '' + ulr;
            var idx = prefix.indexOf('/', prefix.charAt(0) === '/' ? 1 : 0);
            prefix = prefix.substring(0, idx);
            return prefix;
        }



        that.navigateSetController = function (controller) {
            navigateData.controllers[navigateData.url] = controller;
        }

        that.navigateSetNotifyEventController = function (prefix, controller) {
            navigateData.controllers.notifyEvent[prefix] = controller;
        }

        that.navigateNotifyEventController = function () {
            var notifyEvent = navigateData.controllers.notifyEvent;
            for (var key in notifyEvent) {
                if (Object.prototype.hasOwnProperty.call(notifyEvent, key)) {
                    var controller = obj[key];
                    if (controller)
                        controller.ApplyLayout();
                }
            }
        }


        that.navigate = function (options) {
            that.showLoading();
            var url = options.url;
            if (url === undefined || url === '') {
                that.showError("Не задана ссылка для перехода.");
                that.hideLoading();
                return;
            }

            if (navigateData.url === options.url) {
                that.hideLoading();
                return;
            }
            //// заносим ссылку в историю
            //if (options && options.isPush === true && navigateData.url && navigateData.url !== "/nav/curpage")
            //    navigateHistory.push(navigateData.url);
            if (url !== "/nav/curpage")
                navigateData.url = url;

            var prefix = navigateGetPrefix(url);
            var content = navigateData.contents[prefix];
            if (content === undefined) {
                that.showError("Не определен элемент для содежимого.");
                that.hideLoading();
                return;
            }

            content.css({ opacity: 0 });
            content.empty();
            dpas.app.getJson({
                url: url, success: function (data) {
                    if (data.result) {
                        // Отобразим представление
                        content.html(data.view);
                        // Инициализируем контроллер
                        var controller = navigateData.controllers[navigateData.url];
                        that.navigateSetNotifyEventController(prefix, controller);
                        if (controller) {
                            controller.Initialize();
                            controller.ApplyLayout();
                        }
                        content.css({ opacity: 1 });

                        that.hideLoading();
                        if (controller) {
                            controller.Navigate(options.target);
                        }
                    }
                    else {
                        if (data.view)
                            that.showError(data.view);
                        else
                            that.showError(data.error);
                    }
                }
            });
        };

        that.resize = function () {
            that.navigateNotifyEventController();
        };
    };

    dpas.app = new dpas.Application();
    return window.app;
})(window.dpas);