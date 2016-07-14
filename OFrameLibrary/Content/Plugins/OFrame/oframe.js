
Array.prototype.clean = function (deleteValue) {
    var length = this.length;

    for (var i = 0; i < length; i++) {
        if (this[i] == deleteValue) {
            this.splice(i, 1);
            i--;
        }
    }
    return this;
};


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
                        debugger;
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
            ErrorUrl: ""
        }, opt);

        return this.each(function () {
            var assignSortEvents = function () {
                var tableHeaderLinks = $(options.TableID + " " + options.HeaderTagHierarchy);
                var tableFooterLinks = $(options.TableID + " " + options.FooterTagHierarchy);

                $.each(tableHeaderLinks, function (itemIndex, object) {
                    $(object).click(function (event) {
                        event.preventDefault();
                        updateGrid(object);
                    });
                });

                $.each(tableFooterLinks, function (itemIndex, object) {
                    $(object).click(function (event) {
                        event.preventDefault();
                        updateGrid(object);
                    });
                });
            };

            var updateGridCallBack = function (response, status) {

                $(options.BlockScreenID).css('display', 'none');

                if (status === "error") {
                    window.location.href = options.ErrorUrl;
                }

                assignSortEvents();

                if (options.CallBackInjectedFunc) {
                    options.CallBackInjectedFunc();
                }
            };

            var updateGrid = function (e) {
                $(options.BlockScreenID).css('display', 'block');

                var url = $(e).attr('href');

                var grid = $(e).parents(options.ID);

                grid.load(url + " " + options.TableID, updateGridCallBack);
            };

            assignSortEvents();
        });
    };

    $.fn.GridSorter = gridSorter;

}(jQuery));

; (function ($) {
    var ajaxForm = function (opt) {
        var options = $.extend({}, {
            Success: function (response, status) {
                alert(response + " | " + status)
            },
            Error: function (response, status) {
                alert(response + " | " + status)
            },
            Begin: function () {
                alert("Before")
            },
            Progress: function (e, object) {
                if (e.lengthComputable) {
                    var progress = object.getElementsByTagName("progress")[0];
                    if (progress) {
                        $(progress).attr({ value: e.loaded, max: e.total });
                    }
                }
            }
        }, opt);

        return this.each(function (index, object) {
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
                        beforeSend: options.Begin,
                        success: options.Success,
                        error: options.Error,
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

(function ($) {
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





////$(document).ready(function () {
////    $('#fileupload').fileupload({
////        dataType: 'json',
////        url: 'URL_GOES_HERE',
////        autoUpload: true,
////        done: function (e, data) {
////            if (data.result.length > 0) {
////                var imgDiv = document.createElement("div");
////                imgDiv.setAttribute("id", "Image_" + data.result[0].ImageID);

////                var imgDelSpan = document.createElement("span");
////                imgDelSpan.setAttribute("onclick", "DeleteImage(this)");
////                imgDelSpan.setAttribute("class", "DeleteButton");
////                imgDelSpan.setAttribute("data-imageid", data.result[0].ImageID);
////                imgDelSpan.innerHTML = "x";

////                var img = document.createElement("img");
////                img.src = data.result[0].Url;

////                imgDiv.appendChild(imgDelSpan);
////                imgDiv.appendChild(img);

////                $("#ImageContainer").append(imgDiv);
////            }
////            else {
////                alert("Upload Error");
////            }
////        }
////    }).on('fileuploadprogressall', function (e, data) {
////        var progress = parseInt(data.loaded / data.total * 100, 10);
////        $('.progress .progress-bar').css('width', progress + '%');
////    });
////});
////<span class="btn btn-success fileinput-button">
////<i class="glyphicon glyphicon-plus"></i>
////<span>Add Images...</span>
////<input id="fileupload" type="file" name="files[]" multiple>
////</span>
////<br /><br />
////<div class="progress">
////<div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;">
////<span class="sr-only">0% complete</span>
////</div>
////</div>
////<div class="col-md-12" id="ImageContainer">
////@if (Model != null)
////{
////foreach (var img in Model)
////{
////<div id="Image_@img.ImageID">
////<span onclick="DeleteImage(this)" class="DeleteButton" data-imageid="@img.ImageID">x</span>
////<img src="@Url.Content(img.ImageURL)" />
////</div>
////}
////}
////</div>