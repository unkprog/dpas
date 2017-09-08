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

        that.showDialog = function (options) {

            var htmlDialog = '<div class="modal">';
            htmlDialog += ' <div class="modal-content">';
            htmlDialog += '    <h5>'; htmlDialog += options.header ? options.header : 'Сообщение'; htmlDialog += '</h5>';
            htmlDialog += '    <div class="row modal-dialog-content" style="max-height:250px; overflow:auto">';
            htmlDialog += '    </div>';
            htmlDialog += ' </div>';
            htmlDialog += ' <div class="modal-footer">';
            if (options.isCancel)
                htmlDialog += '    <button class="modal-dialog-cancel modal-action waves-effect btn-flat" style="margin:0.1em">Отмена</button>';
            htmlDialog += '    <button class="modal-dialog-ok modal-action waves-effect btn-flat" style="margin:0.1em">OK</button>';
            htmlDialog += ' </div>';
            htmlDialog += '</div>';
            
            var modal_dialog = $(htmlDialog);
            $("body").append(modal_dialog);
            modal_dialog.modal({
                dismissible: false,
            });

            var modal_dialog_content = modal_dialog.find(".modal-dialog-content");
            var modal_dialog_ok = modal_dialog.find(".modal-dialog-ok");
            var modal_dialog_cancel = modal_dialog.find(".modal-dialog-cancel");

            if (options.msg)
                modal_dialog_content.html(options.msg);

            var onClose = function (result) {
                modal_dialog_ok.off("click");
                modal_dialog_cancel.off("click");
                modal_dialog.modal('close');
                modal_dialog.remove();
                if (options.callback)
                    options.callback({ dialogResult: result });
            }

            modal_dialog_ok.on("click", function (event) {
                onClose(true);
            });
            modal_dialog_cancel.on("click", function () {
                onClose(false);
            });

            modal_dialog.modal('open');
        }

        that.showError = function(errorMsg){
            this.showDialog({ header: 'Ошибка', msg: errorMsg });
        }

        function alertError(num) {
            //ошибки запроса
            var arr = ['Ваш браузер не поддерживает Ajax', 'Не удалось выполнить запрос', 'Адрес не существует', 'Время ожидания истекло']; //['Your browser does not support Ajax', 'Request failed', 'Address does not exist', 'The waiting time left'];
            that.showDialog({ msg: arr[num] });
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
                        that.showDialog({ msg: xhr.responseText });
                    else
                        that.showDialog({ msg: thrownError });
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
                    var controller = navigateData.controllers.notifyEvent[key];
                    if (controller)
                        controller.ApplyLayout();
                }
            }
        }


        that.navigate = function (options) {
            that.showLoading();
            var url = options.url;
            if (url === undefined || url === '') {
                that.showDialog({ msg: "Не задана ссылка для перехода." });
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
                that.showDialog({ msg: "Не определен элемент для содежимого." });
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
                            controller.Navigate(options.target);
                            controller.Initialize();
                            controller.ApplyLayout();
                        }
                        content.css({ opacity: 1 });

                        that.hideLoading();
                    }
                    else {
                        if (data.view)
                            that.showDialog({ msg: data.view });
                        else
                            that.showDialog({ msg: data.error });
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