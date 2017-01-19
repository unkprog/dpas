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

        that.callJson = function (options) {
            loading.show();

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

        that.loadData = function (options) {
            loading.show();

            $.ajax({
                type: options.type, url: options.url, async: true, dataType: options.dataType,
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

        that.getData = function (url, callback) {
            that.getDataParams(url, undefined, callback);
            //var location = '' + (window.history.location || window.location);
            //var params = this.getParams(location);
            //$.ajax({
            //    url: url, type: "POST", cache: false, async: true, data: params,
            //    success: function (data) {
            //        if (callback)
            //            callback(data);
            //    },
            //    error: alertError
            //});
        };

        that.getDataParams = function (url, data, callback) {
            var locationPath = '' + (window.history.location || window.location);
            var params = data;
            if (!params) {
                locationPath = '' + (window.history.location || window.location);
                params = that.getParams(locationPath);
            }

            $.ajax({
                url: url, type: "POST", cache: false, async: true, data: params,
                success: function (data) {
                    if (callback)
                        callback(data);
                },
                error: alertError
            });
        };

        that.getParams = function (url) {
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
    };
    dpas.app = new dpas.Application();
    return window.app;
})(window.dpas);