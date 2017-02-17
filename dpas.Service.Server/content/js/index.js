
$(document).ready(function () {
    var loading = $('.dpas-loadbar');
    loading.show();

    //$.ajax({
    //    url: "/mvc/dpas.application.js",
    //    dataType: "script",
    //    success: function (script, textStatus, jqXHR) {
            dpas.app.setLoadingElement(loading);
            var content = $("#content");
            dpas.app.navigateSetContent('/nav', content);

            (function (eventInfo) {

                function getReference(target) {
                    if (target === undefined || target === null) return undefined;
                    if (target.nodeName === 'A') return target;
                    var parent = target.parentNode;
                    while (parent !== undefined && parent !== null) {
                        if (parent.nodeName === 'A') return parent;
                        parent = parent.parentNode;
                    }
                }

                /*
                 * Получаем нормальный объект Location
                 * заметьте, это единственная разница при работе с данной библиотекой,
                 * так как объект window.location нельзя перезагрузить, поэтому
                 * библиотека history возвращает сформированный "location" объект внутри
                 * объекта window.history, поэтому получаем его из "history.location".
                 * Для браузеров поддерживающих "history.pushState" получаем
                 * сформированный объект "location" с обычного "window.location".
                 */
                var location = window.history.location || window.location;

                // вешаем события на все ссылки в нашем документе
                document[eventInfo[0]](eventInfo[1] + 'click', function (event) {
                    event = event || window.event;
                    var target = getReference(event.target || event.srcElement);
                    // ищем все ссылки с классом 'ajax'
                    if (target && target.nodeName === 'A' && (' ' + target.className + ' ').indexOf('ajax') >= 0) {

                        var r = target.href;
                        if (r !== '') {

                            r = r.replace(location.protocol + '//' + location.host, '');
                            // тут можете вызвать подгрузку данных и т.п.
                            dpas.app.navigate({ url: r, target: target });
                            // не даем выполнить действие по умолчанию
                            if (event.preventDefault) {
                                event.preventDefault();
                            } else {
                                event.returnValue = false;
                            }
                        }
                    }
                }, false);

                // вешаем событие на popstate которое срабатывает при нажатии back/forward в браузере
                window[eventInfo[0]](eventInfo[1] + 'popstate', function (event) {
                    //navigate(event.target.currentUrl, event.target, false);
                    // тут можете вызвать подгрузку данных и т.п.
                    //var ddd = 0;
                    //navigate(url, el, isAmber);
                    // просто сообщение
                    // alert("We returned to the page with a link: " + location.href);
                }, false);
            })(window.addEventListener ? ['addEventListener', ''] : ['attachEvent', 'on']);

            dpas.app.navigate({ url: "/nav/curpage" });
    //    }
    //});

});