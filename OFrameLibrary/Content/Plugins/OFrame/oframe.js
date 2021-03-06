﻿Array.prototype.clean = function (deleteValue) {
    var length = this.length;

    for (var i = 0; i < length; i++) {
        "ignore";
        if (this[i] == deleteValue) {
            this.splice(i, 1);
            i--;
        }
    }
    return this;
};

Array.prototype.indexOf = function (obj, fromIndex) {
    if (fromIndex == null) {
        fromIndex = 0;
    } else if (fromIndex < 0) {
        fromIndex = Math.max(0, this.length + fromIndex);
    }
    for (var i = fromIndex, j = this.length; i < j; i++) {
        if (this[i] == obj)
            return i;
    }
    return -1;
};

Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};

$.fn.clearErrors = function () {
    $(this).each(function () {
        $(this).removeClass("input-validation-error");
        var span = $("span[data-valmsg-for='" + this.id + "']");
        $(span).html("");
    });
};

var oframe = {
    isNullOrWhiteSpace: function (str) {
        return str === null || str.match(/^ *$/) !== null;
    },
    toDate: function (dateStr, spltChar) {
        var spch = "/";
        if (spltChar) {
            spch = spltChar;
        }
        var parts = dateStr.split(spch);
        return new Date(parts[2], parts[1] - 1, parts[0]);
    },
    treatAsUTC: function (date) {
        var result = new Date(date);
        result.setMinutes(result.getMinutes() - result.getTimezoneOffset());
        return result;
    },
    daysBetween: function (startDate, endDate) {
        var millisecondsPerDay = 24 * 60 * 60 * 1000;
        return (this.treatAsUTC(endDate) - this.treatAsUTC(startDate)) / millisecondsPerDay;
    },
    getArray: function (field) {
        var value = $(field).val();
        var arr = value.split(";");
        arr.clean("");
        return arr;
    },
    isNumber: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },
    rebindValidator: function (selector) {
        var form = $("form" + selector);
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(form);
    },
    format: function () {
        // The string containing the format items (e.g. "{0}")
        // will and always has to be the first argument.
        var theString = arguments[0];

        // start with the second argument (i = 1)
        for (var i = 1; i < arguments.length; i++) {
            // "gm" = RegEx options for Global search (more than one instance)
            // and for Multiline search
            var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
            theString = theString.replace(regEx, arguments[i]);
        }

        return theString;
    },
    createBlockTag: function () {
        var html = $.parseHTML("<div id='__BlockScreen__' style='background-color:rgba(0,0,0,0.3); position: fixed; width: 100%; height: 100%; top: 0; left: 0; display:none; z-index:9999999;'><div class='loader'></div></div>");

        var blockScreen = $("#__BlockScreen__");

        if (!blockScreen.length) {
            $("body").append(html);
        }
    },
    blockUI: function () {
        this.createBlockTag();
        document.getElementById("__BlockScreen__").style.display = "block";
    },
    unblockUI: function () {
        this.createBlockTag();
        document.getElementById("__BlockScreen__").style.display = "none";
    },
    formsValidate: function (selector) {
        var valid = true;
        var id = "";
        var arr = [];

        var forms = $("form" + selector);

        forms.each(function (index, object) {
            if (!$(object).valid()) {
                valid = false;
                id = object.id;
                return false;
            }
            else if (!object.action && object.id) {
                arr.push($(object).serializeObject());
            }
        });

        return {
            valid: valid,
            id: id,
            arr: arr
        };
    },
    inovokeFunction: function (fnc, arg) {
        if (fnc !== "") {
            var fn = window[fnc];
            if (typeof fn === 'function') {
                fn(arg);
            }
        }
    },
    openTab: function (id) {
        var tabContent = $("#" + id).parent(".tab-pane");

        $(tabContent).parents(".tc-tabs").find(".nav-tabs li a[data-tabid=" + $(tabContent).attr("id") + "]").click();
    },
    updateUrl: function (uri, key, value) {
        //var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        //var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        //if (uri.match(re)) {
        //    return uri.replace(re, '$1' + key + "=" + value + '$2');
        //}
        //else {
        //    return uri + separator + key + "=" + value;
        //}

        var i = uri.indexOf('#');
        var hash = i === -1 ? '' : uri.substr(i);
        uri = i === -1 ? uri : uri.substr(0, i);

        var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.match(re)) {
            uri = uri.replace(re, '$1' + key + "=" + value + '$2');
        } else {
            uri = uri + separator + key + "=" + value;
        }
        return uri + hash;  // finally append the hash as well

        //REMOVE KEY VALUE PAIR IF VALUE IS EMPTY
        //var i = uri.indexOf('#');
        //var hash = i === -1 ? '' : uri.substr(i);
        //uri = i === -1 ? uri : uri.substr(0, i);

        //var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
        //var separator = uri.indexOf('?') !== -1 ? "&" : "?";

        //if (!value) {
        //    // remove key-value pair if value is empty
        //    uri = uri.replace(new RegExp("([?&]?)" + key + "=[^&]*", "i"), '');
        //    if (uri.slice(-1) === '?') {
        //        uri = uri.slice(0, -1);
        //    }
        //    // replace first occurrence of & by ? if no ? is present
        //    if (uri.indexOf('?') === -1) uri = uri.replace(/&/, '?');
        //} else if (uri.match(re)) {
        //    uri = uri.replace(re, '$1' + key + "=" + value + '$2');
        //} else {
        //    uri = uri + separator + key + "=" + value;
        //}
        //return uri + hash;
    },
    select: function (arg, field) {
        var value = $(field).val();
        var arr = value.split(";");
        arr.clean("");
        var index = arr.indexOf(arg);
        if (index === -1) {
            $(field).val(value + arg + ";");
        }
    },
    deSelect: function (arg, field) {
        var value = $(field).val();
        var arr = value.split(";");
        arr.clean("");
        var index = arr.indexOf(arg);
        if (index !== -1) {
            arr.splice(index, 1);
            var val = arr.join(";");
            if (arr.length > 0) {
                val += ";";
            }
            $(field).val(val);
        }
    }
};

; (function ($) {
    $.fn.serializeObject = function () {
        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push": /^$/,
                "fixed": /^\d+$/,
                "named": /^[a-zA-Z0-9_]+$/
            };

        this.build = function (base, key, value) {
            base[key] = value;
            return base;
        };

        this.push_counter = function (key) {
            if (push_counters[key] === undefined) {
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };

        $.each($(this).serializeArray(), function () {
            // skip invalid keys
            if (!patterns.validate.test(this.name)) {
                return;
            }

            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;

            while ((k = keys.pop()) !== undefined) {
                // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

                // push
                if (k.match(patterns.push)) {
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }

                // fixed
                else if (k.match(patterns.fixed)) {
                    merge = self.build([], k, merge);
                }

                // named
                else if (k.match(patterns.named)) {
                    merge = self.build({}, k, merge);
                }
            }

            json = $.extend(true, json, merge);
        });

        return json;
    };
})(jQuery);

; (function ($) {
    var toggler = function (opt) {
        var options = $.extend({}, {
            ID: "#DaySelector",
            DataAttributeName: "day",
            OnSelected: function (args) { },
            OnUnSelected: function (args) { }
        }, opt);

        return this.each(function () {
            var anchors = $("#" + this.id + " ul a");;

            var valueField = $("#" + options.ID)[0];

            var clickedData = {};

            var clickedElement = {};

            var bindClickEvent = function (object) {
                $(object).click(function () {
                    toggle(object);
                });
            };

            var setValue = function (value) {
                $(valueField).val(value);
            };

            var getValue = function () {
                return $(valueField).val();
            };

            var getArray = function () {
                return getValue().split(';').clean('');
            };

            var init = function (e) {
                clickedElement = $(e);
                clickedData = clickedElement.data(options.DataAttributeName);
                array = getArray();
                length = array.length;
            };

            var toggle = function (e) {
                init(e);

                var data = clickedData.toString();

                if (length) {
                    $.each(array, function (index, value) {
                        "ignore";
                        if (value == data) {
                            remove(data);
                            return false;
                        }
                        else {
                            if (!exists(data)) {
                                add(data);
                                return false;
                            }
                        }
                    });
                }
                else {
                    add(data);
                }
            };

            var exists = function (data) {
                if ($.inArray(data, array) === -1) {
                    return false;
                }
                else {
                    return true;
                }
            };

            var add = function (data) {
                clickedElement.addClass("Selected");

                var arr = array;

                arr.push(data);

                setValue(arr.join(';'));

                options.OnSelected(data);
            };

            var remove = function (data) {
                clickedElement.removeClass("Selected");

                var oldArr = array;

                var newArr = $.grep(oldArr, function (value) {
                    return value !== data;
                });

                var newValue = newArr.join(';');

                setValue(newValue);

                options.OnUnSelected(data);
            };

            var array = getArray();

            var length = array.length;

            $.each(anchors, function (itemIndex, object) {
                bindClickEvent(object);
                var data = $(object).data(options.DataAttributeName);
                if (exists(data)) {
                    $(object).addClass("Selected");
                }
            });
        });
    };

    $.fn.Toggler = toggler;
}(jQuery));

; (function ($) {
    var gridSorter = function (opt) {
        var options = $.extend({}, {
            ID: "#GridContainer",
            TableID: "#TableContainer",
            HeaderTagHierarchy: "table thead tr th a",
            FooterTagHierarchy: "table tfoot tr td a",
            CallBackInjectedFunc: undefined,
            BlockScreenID: "#__BlockScreen__",
            ErrorUrl: "",
            ValuesFieldSelector: "",
            SelectMode: "single", //multiple, none
            AllRowSelector: ".allRowSelector",
            RowSelector: ".rowSelector",
            OnRowSelected: function (e) {
                alert($(e).data("id"));
            }
        }, opt);

        var assignSortEvents = function (selector) {
            var tableHeaderLinks = $(selector).find(options.TableID + " " + options.HeaderTagHierarchy);
            var tableFooterLinks = $(selector).find(options.TableID + " " + options.FooterTagHierarchy);

            $.each(tableHeaderLinks, function (itemIndex, object) {
                $(object).click(function (event) {
                    event.preventDefault();
                    updateGrid(event.currentTarget, selector);
                });
            });

            $.each(tableFooterLinks, function (itemIndex, object) {
                $(object).click(function (event) {
                    event.preventDefault();
                    updateGrid(event.currentTarget, selector);
                });
            });

            //if (options.SelectMode === "single") {
            //}
            //else if (options.SelectMode === "multiple") {
            //}

            //$(chkbox).click(function (event) {
            //    options.OnRowSelected(event.currentTarget);
            //});
        };

        var updateGridCallBack = function (response, status, selector) {
            oframe.unblockUI();

            if (status === "error") {
                window.location.href = options.ErrorUrl;
            }

            assignSortEvents(selector);

            if (options.CallBackInjectedFunc) {
                options.CallBackInjectedFunc();
            }
        };

        var updateGrid = function (e, selector) {
            oframe.blockUI();

            var url = $(e).attr('href');

            var grid = $(e).parents(options.ID);

            grid.load(url + " " + options.TableID, function (response, status) {
                updateGridCallBack(response, status, selector);
            });
        };

        return this.each(function (index, object) {
            assignSortEvents(object);
        });
    };

    $.fn.GridSorter = gridSorter;
}(jQuery));

; (function ($) {
    var ajaxForm = function (opt) {
        var options = $.extend({}, {
            Selector: "",
            Load: function (form) {
            },
            Success: function (response, status) {
            },
            Error: function (response, status) {
            },
            Begin: function (formData) {
                return true;
            },
            Progress: function (e, object) {
                if (e.lengthComputable) {
                    var progress = $(".progressBar");
                    if (progress) {
                        var per = ((e.loaded * 100) / e.total);
                    }
                }
            }
        }, opt);

        return this.each(function (index, object) {
            options.Load(object);

            oframe.rebindValidator(options.Selector);

            $(object).submit(function () {
                if ($(object).valid()) {
                    var form = $(object);
                    var formData = false;

                    if (window.FormData) {
                        formData = new FormData(form[0]);
                    }

                    $.ajax({
                        url: object.action,
                        type: object.method,
                        xhr: function () {
                            var myXhr = $.ajaxSettings.xhr();
                            if (myXhr.upload) {
                                myXhr.upload.addEventListener('progress', function (e) {
                                    options.Progress(e, object);
                                }, false);
                            }
                            return myXhr;
                        },
                        beforeSend: function () {
                            var x = object;
                            oframe.blockUI();
                            var vr = oframe.formsValidate(options.Selector);

                            if (!vr.valid) {
                                oframe.openTab(vr.id);
                                oframe.unblockUI();
                                return false;
                            }

                            formData.append("Translations", JSON.stringify(vr.arr));

                            return options.Begin(formData);
                        },
                        success: function (response, status, xhr) {
                            oframe.unblockUI();
                            options.Success(response, status, xhr);
                        },
                        error: function (response, status) {
                            oframe.unblockUI();
                            options.Error(response, status);
                        },
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false
                    });
                }
                return false;
            });
        });
    };

    $.fn.AjaxForm = ajaxForm;
}(jQuery));

; (function ($) {
    $.fn.ngResponsiveTables = function (options) {
        var defaults = {
            smallPaddingCharNo: 5,
            mediumPaddingCharNo: 10,
            largePaddingCharNo: 15
        },
            $selElement = this,
            ngResponsiveTables = {
                opt: '',
                dataContent: '',
                globalWidth: 0,
                init: function () {
                    this.opt = $.extend(defaults, options);
                    ngResponsiveTables.targetTable();
                },
                targetTable: function () {
                    var that = this;
                    $selElement.find('tr').each(function () {
                        $(this).find('td').each(function (i, v) {
                            that.checkForTableHead($(this), i);
                            $(this).addClass('tdno' + i);
                        });
                    });
                },
                checkForTableHead: function (element, index) {
                    if ($selElement.find('th').length) {
                        this.dataContent = $selElement.find('th')[index].textContent;
                    } else {
                        this.dataContent = $selElement.find('tr:first td')[index].textContent;
                    }
                    // This padding is for large texts inside header of table
                    // Use small, medium and large paddingMax values from defaults to set-up offsets for each class
                    if (this.opt.smallPaddingCharNo > $.trim(this.dataContent).length) {
                        element.addClass('small-padding');
                    } else if (this.opt.mediumPaddingCharNo > $.trim(this.dataContent).length) {
                        element.addClass('medium-padding');
                    } else {
                        element.addClass('large-padding');
                    }
                    element.attr('data-content', this.dataContent);
                }
            };

        $(function () {
            ngResponsiveTables.init();
        });
        return this;
    };
}(jQuery));

; (function ($) {
    var localizeForm = function (opt) {
        var options = $.extend({}, {
            LocaleSelectorUrl: "",
            FormName: "",
            FormUrl: "",
            LocaleButtonSelector: "a.localeButton",
            PopUpSelector: ".localeListPopup",
            LocalesFieldSelector: "input.selectedLocales",
            LocalesListSelector: "ul.localesList",
            LocaleItemSelector: "a.localeItem",
            PreLoad: false,
            CurrentLocale: "en-US",
            CurrentLocaleName: "English"
        }, opt);

        var openPopUp = function (options, selector) {
            var url = oframe.updateUrl(options.LocaleSelectorUrl, "Locale", options.CurrentLocale);
            $.magnificPopup.open({
                items: {
                    src: $(selector).find(options.PopUpSelector)
                },
                type: 'inline',
                closeBtnInside: false,
                midClick: true,
                callbacks: {
                    open: function () {
                        var jsonData = {
                            Locales: getHashTruncatedLocales($(selector).find(options.LocalesFieldSelector))
                        };

                        $.ajax({
                            url: url,
                            type: "POST",
                            beforeSend: function () {
                                oframe.blockUI();
                            },
                            success: function (response) {
                                $(options.PopUpSelector).html(response);

                                $(options.LocalesListSelector + " " + options.LocaleItemSelector).on("click", function (event) {
                                    localeSelected(event, selector);
                                });

                                oframe.unblockUI();
                            },
                            error: function () {
                                $.magnificPopup.close();
                                oframe.unblockUI();
                            },
                            data: jsonData
                        });
                    },
                    close: function () {
                    }
                }
            });
        }

        var localeSelected = function (event, selector) {
            preAddTab({
                Selector: selector,
                LocaleItem: event.currentTarget,
                LocaleButtonSelector: options.LocaleButtonSelector,
                FormName: options.FormName,
                LocalesFieldID: options.LocalesFieldSelector
            });
        }

        var preAddTab = function (args) {
            $.magnificPopup.close();
            oframe.blockUI();

            var lang = getLanguage(args.LocaleItem);

            args = $.extend(args, {
                Lang: lang,
                TabID: oframe.format('Tab_{0}_{1}', args.FormName, lang.Locale)
            });

            var localeHash = oframe.format("{0}#{1}", lang.Locale, lang.Name);
            oframe.select(localeHash, $(args.Selector).find(options.LocalesFieldSelector));

            addTab($(args.Selector).find(args.LocaleButtonSelector), args);

            var url = oframe.updateUrl(options.FormUrl, "Locale", lang.Locale);
            url = oframe.updateUrl(url, "FormName", options.FormName);
            $(args.Selector).find("#" + args.TabID).load(url, function () {
                oframe.unblockUI();

                var form = $(args.Selector).find("#" + args.TabID + " form." + options.FormName);
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
            });
        }

        var addTab = function (e, args) {
            $(args.Selector).find(".tab-content div").removeClass("active");
            $(args.Selector).find(".nav-tabs li").removeClass("active");

            var li = '<li class="active">';
            li += '<a class="tabAnchor" href="#{0}" data-toggle="tab" ';
            li += 'data-tabid="{0}" ';
            li += 'data-locale="{1}" ';
            li += 'data-name="{2}">';
            li += '{2}&nbsp;';
            li += '<i class="close fa fa-close"></i></a></li>';

            li = oframe.format(li, args.TabID, args.Lang.Locale, args.Lang.Name);
            var tab = oframe.format('<div class="tab-pane active" id="{0}"></div>', args.TabID);

            $(e).closest('li').before(li);
            $(args.Selector).find('.tab-content').append(tab);

            $(args.Selector).find("i.close").on("click", function (event) {
                tabClosed(event.currentTarget, args.Selector);
            });
        }

        var tabClosed = function (e, selector) {
            var anchor = $(e).parent('a.tabAnchor');
            var tabID = $(anchor).data("tabid");
            var locale = $(anchor).data("locale");
            var name = $(anchor).data("name");

            var tab = $(selector).find("#" + tabID);
            $(tab).remove();

            $(anchor).parent().remove();

            var localeHash = oframe.format("{0}#{1}", locale, name);

            oframe.deSelect(localeHash, $(selector).find(options.LocalesFieldSelector));

            $(selector).find(".tab-content div").removeClass("active");
            $(selector).find(".nav-tabs li").removeClass("active");
            $(selector).find('.nav-tabs li:first-child a').click();
        }

        var getHashTruncatedLocales = function (e) {
            var selLocArr = $(e).val().split(";").clean("");

            var locales = "";

            $.each(selLocArr, function (index, object) {
                locales = locales + object.split("#").splice(0, 1) + ";";
            });

            return locales;
        }

        var getLanguage = function (e) {
            return {
                Name: $(e).data("name"),
                Locale: $(e).data("locale"),
                Direction: $(e).data("direction")
            };
        }

        var getLocaleName = function (locale, locales) {
            var name = "Undefined";
            var arr = locales.split(";");
            $.each(arr, function (index, object) {
                var loc = object.split("#")[0];
                if (loc === locale) {
                    name = object.split("#")[1];
                    return;
                }
            });

            return name;
        }

        var initializeTabs = function (selector) {
            var localesField = $(selector).find(options.LocalesFieldSelector);
            var locales = getHashTruncatedLocales(localesField).split(";").clean("").clean(options.CurrentLocale);

            $.each(locales, function (index, locale) {
                oframe.blockUI();

                var tabID = oframe.format('Tab_{0}_{1}', options.FormName, locale);

                var name = getLocaleName(locale, $(localesField).val());

                var li = '<li>';
                li += '<a class="tabAnchor" href="#{0}" data-toggle="tab" ';
                li += 'data-tabid="{0}" ';
                li += 'data-locale="{1}" ';
                li += 'data-name="{2}">';
                li += '{2}&nbsp;';
                li += '<i class="close fa fa-close"></i></a></li>';

                li = oframe.format(li, tabID, locale, name);
                var tab = oframe.format('<div class="tab-pane" id="{0}"></div>', tabID);

                $(selector).find(options.LocaleButtonSelector).closest('li').before(li);
                $(selector).find('.tab-content').append(tab);

                $(selector).find("i.close").on("click", function (event) {
                    tabClosed(event.currentTarget, selector);
                });

                var url = oframe.updateUrl(options.FormUrl, "Locale", locale);
                url = oframe.updateUrl(url, "FormName", options.FormName);

                $(selector).find("#" + tabID).load(url, function () {
                    oframe.unblockUI();
                    var form = $(selector).find("#" + tabID + " form." + options.FormName);
                    form.removeData('validator');
                    form.removeData('unobtrusiveValidation');
                    $.validator.unobtrusive.parse(form);
                });
            });
        }

        var buildTabStructure = function (selector) {
            oframe.blockUI();
            var ulTag = '<ul class="nav nav-tabs tab-color-dark background-dark">';
            ulTag += '<li class="active"><a href="#Tab_{0}-{1}" data-toggle="tab">{2}</a></li>';
            ulTag += '<li>';
            ulTag += '<a class="tooltip-info localeButton" data-placement="top" data-rel="tooltip" data-original-title="Add Language"><i class="fa fa-plus-circle"></i></a>'
            ulTag += '</li>';
            ulTag += '</ul>'

            ulTag = oframe.format(ulTag, options.FormName, options.CurrentLocale, options.CurrentLocaleName);

            var tabContentTag = '<div class="tab-content">';
            tabContentTag += '<div class="tab-pane active" id="Tab_{0}-{1}">';
            tabContentTag += '</div>';
            tabContentTag += '</div>';

            tabContentTag = oframe.format(tabContentTag, options.FormName, options.CurrentLocale);

            var popTag = '<div class="localeListPopup mfp-hide"></div>';

            $(selector).append(ulTag);
            $(selector).append(tabContentTag);
            $(selector).append(popTag);

            var tabID = oframe.format("#Tab_{0}-{1}", options.FormName, options.CurrentLocale);
            var tab = $(selector).find(tabID);

            var form = $(selector).find("form." + options.FormName);

            $(form).appendTo(tab);

            oframe.unblockUI();
        };

        return this.each(function (index, object) {
            buildTabStructure(object);

            $(object).find(options.LocaleButtonSelector).on("click", function () {
                openPopUp(options, object);
            });

            if (options.PreLoad) {
                initializeTabs(object);
            }
        });
    };

    $.fn.LocalizeForm = localizeForm;
}(jQuery));

var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

dateFormat.i18n = {
    dayNames: [
        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};
