var content, loading, modal_error, modal_error_content; //, nav_desktop, nav_desktop_a_spans, div_top_offset;

var currentUrl;
function navigateClear()
{
    currentUrl = '';
}

function navigate(url, el, isAmber) {
    if (currentUrl !== url)  {
        loading.show();

        content.css({ opacity: 0 });
        content.empty();
        dpas.app.loadHtml(url,
                   function (html) {
                       currentUrl = url;
                       content.html(html);
                       //var scripts = content[0].getElementsByTagName("script");
                       //for (var i = 0; i < scripts.length; ++i) {
                       //    var script = scripts[i];
                       //    eval(script.innerText);
                       //}

                       //////var elAmber = isAmber === true ? el : $(".a-index");
                       //////nav_desktop_a_spans.removeClass('amber-text').addClass('white-text');
                       //////$(el).find('span').removeClass('white-text').addClass('amber-text');
                       content.css({ opacity: 1 });
                       //Materialize.fadeInImage(content);
                       loading.hide();
                   });
    }
}
function showError(msg) {
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

$(document).ready(function () {
    loading = $('.dpas-loadbar');
    loading.show();
    content = $("#content");

    //$.ajax({
    //    url: "/mvc/dpas.application.js",
    //    dataType: "script",
    //    success: function (script, textStatus, jqXHR) {

           
            ////////nav_desktop = $("#nav-desktop");
            ////////nav_desktop_a_spans = nav_desktop.find('a span');
            ////////div_top_offset = $("#div-top-offset");
            ////////div_top_offset.height($("#nav-header-navigation").height());
            //////////$("#nav-desktop").on("click", "a", function (event) {
            //////////    event.preventDefault();
            //////////    navigate(event.currentTarget.href, event.currentTarget, true);
            //////////});
            var el = $(".a-index");
            //////////$('#logo-container').on("click", function (event) {
            //////////    event.preventDefault();
            //////////    navigate(el[0].href, el[0], false);
            //////////});
           

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
                // получаем нормальный объект Location

                /*
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
                            // заносим ссылку в историю
                            history.pushState({ url: r }, null, r);
                            //history.replaceState({ path: r }, '');
                            // тут можете вызвать подгрузку данных и т.п.
                            navigate(r, target, false);
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

                    navigate(event.target.location, event.target, false);
                    // тут можете вызвать подгрузку данных и т.п.
                    var ddd = 0;
                    //navigate(url, el, isAmber);
                    // просто сообщение
                    // alert("We returned to the page with a link: " + location.href);
                }, false);
            })(window.addEventListener ? ['addEventListener', ''] : ['attachEvent', 'on']);

            navigate("/nav/curpage", el[0]);
      //  }
   // });





});