QCC =
{
    initializers: new Array(),
    addInit: function (method) {
        QCC.initializers[QCC.initializers.length] = method;
    },
    initialized: false,
    init: function (elements) {
        if (elements == null || elements.length == 0)
            elements = $(document);

        elements = $(elements);
        elements.find(".Tabs").tabs();
        elements.find(".Accordion").each(function () {
            QCC.ui.makeAccordion($(this));
        });
        elements.find(".Button").each(function () {
            QCC.ui.makeButton($(this));
        });
        elements.find(".Buttonset, .Radio").each(function () {
            QCC.ui.makeButtonset($(this));
        });
        elements.find(".Sortable").each(function () {
            QCC.ui.makeSortable($(this));
        });
        elements.find(".Draggable").each(function () {
            QCC.ui.makeDraggable($(this));
        });
        elements.find(".ProgressBar").each(function () {
            QCC.ui.makeProgressBar($(this));
        });
        elements.find("input.Date").each(function () {
            QCC.ui.makeDatePicker($(this));
        });
        elements.find("#Dialog, div.Dialog").each(function () {
            QCC.ui.makeDialog($(this));
        });
        elements.find(".SplitButton .DropDown").click(function () {
            var dropDown = $(this);
            var splitButton = $(dropDown.parent(".SplitButton"));
            var menu = $(splitButton.find("ul"));
            var parent = dropDown.parent();
            var menuPosition =
            {
                my: (menu.data("position") != null && menu.data("position").my != null ? menu.data("position").my : "right top"),
                at: (menu.data("position") != null && menu.data("position").at != null ? menu.data("position").at : "right bottom"),
                of: (menu.data("position") != null && menu.data("position").of != null ? $(parent).find(menu.data("position").of) : dropDown),
                collision: (menu.data("position") != null && menu.data("position").collision != null ? menu.data("position").collision : "none none")
            };

            splitButton.addClass("Active");
            menu.show();
            menu.position(menuPosition);
            $(document).one("click", function () {
                splitButton.removeClass("Active");
                menu.hide();
            });

            return false;
        });
        elements.find("#Message").show();
        elements.find("menu li").has("ul").hover(function () {
            var parent = $(this);

            parent.children("ul").each(function () {
                var child = $(this);
                var position = child.data("position");

                position =
                {
                    my: (position == null ? "top left" : position.my),
                    at: (position == null ? "bottom left" : position.at)
                };

                child.position(position);
            });
        });
        elements.find("menu li").has("ul a.Selected").children("a").addClass("Selected");

        //Execute the page initialization function(s).
        if (!QCC.initialized && QCC.initializers != null)
            for (i = 0; i < QCC.initializers.length; i++)
                QCC.initializers[i]();

        QCC.initialized = true;
    },
    ui:
    {
        makeAccordion: function (obj) {
            obj.accordion(QCC.ui.extractJQueryOptions(obj));
        },
        makeButton: function (obj) {
            obj.button(QCC.ui.extractJQueryOptions(obj));
        },
        makeButtonset: function (obj) {
            obj.buttonset(QCC.ui.extractJQueryOptions(obj));
        },
        makeDatePicker: function (obj) {
            obj.datepicker(QCC.ui.extractJQueryOptions(obj));
        },
        makeProgressBar: function (obj) {
            obj.progressbar(QCC.ui.extractJQueryOptions(obj));
        },
        makeDialog: function (obj) {
            var options = QCC.ui.extractJQueryOptions(obj);

            if (options != null && options.modal) {
                options.buttons =
                {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                };
            }

            obj.dialog(options);
        },
        makeMenu: function (obj) {
            obj.menu(QCC.ui.extractJQueryOptions(obj));
        },
        extractJQueryOptions: function (obj) {
            var attribute = obj.attr("data-options");

            if (attribute != null)
                return $.parseJSON(attribute);

            return null;
        }
    }
};

function SelectBlockButton(obj) {
    var li = $(obj);
    var siblings = li.siblings();

    li.siblings().removeClass("Selected");
    li.addClass("Selected");
}