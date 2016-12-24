(function ($) {
    $.fn.treemenu = function (a_options) {
        var that = this;
        var options = a_options || {};
        options.delay = options.delay || 0;
        options.openActive = options.openActive || false;
        options.closeOther = options.closeOther || false;
        options.activeSelector = options.activeSelector || ".dpas-tree-active";

        that.addClass("dpas-treemenu");

        if (!options.nonroot) {
            that.addClass("dpas-treemenu-root");
        }

        options.nonroot = true;

        that.find("> li").each(function () {
            e = $(this);
            var subtree = e.find('> ul');
            var button = e.find('.dpas-toggler').eq(0);

            if (button.length == 0) {
                // create toggler
                var button = $('<span>');
                button.addClass('dpas-toggler');
                e.prepend(button);
            }

            if (subtree.length > 0) {
                subtree.hide();

                e.addClass('dpas-tree-closed');

                e.find(button).click(function () {
                    var li = $(this).parent('li');

                    if (options.closeOther && li.hasClass('dpas-tree-closed')) {
                        var siblings = li.parent('ul').find("li:not(.dpas-tree-empty)");
                        siblings.removeClass("dpas-tree-opened");
                        siblings.addClass("dpas-tree-closed");
                        siblings.removeClass(options.activeSelector);
                        siblings.find('> ul').slideUp(options.delay);
                    }

                    li.find('> ul').slideToggle(options.delay);
                    li.toggleClass('dpas-tree-opened');
                    li.toggleClass('dpas-tree-closed');
                    li.toggleClass(options.activeSelector);
                });

                $(this).find('> ul').treemenu(options);
            } else {
                $(this).addClass('dpas-tree-empty');
            }
        });

        //if (options.openActive) {
        //    var cls = this.attr("class");

        //    this.find(options.activeSelector).each(function () {
        //        var el = $(this).parent();

        //        while (el.attr("class") !== cls) {
        //            el.find('> ul').show();
        //            if (el.prop("tagName") === 'UL') {
        //                el.show();
        //            } else if (el.prop("tagName") === 'LI') {
        //                el.removeClass('tree-closed');
        //                el.addClass("tree-opened");
        //                el.show();
        //            }

        //            el = el.parent();
        //        }
        //    });
        //}

        return that;
    }
})(jQuery);
