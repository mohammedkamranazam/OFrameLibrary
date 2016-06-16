
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

var GridSorter = GridSorter || {};

GridSorter = {
    CallBackInjectedFunc: undefined,

    ErrorUrl: "",

    AssignSortEvents: function () {

        var tableHeaderLink = $('#TableContainer table thead tr th a');
        var tableFooterLink = $('#TableContainer table tfoot tr td a');

        tableHeaderLink.attr("onclick", "return GridSorter.UpdateGrid(this)");
        tableFooterLink.attr("onclick", "return GridSorter.UpdateGrid(this)");
    },

    UpdateGridCallBack: function (response, status) {

        $("#__BlockScreen__").css('display', 'none');

        if (status === "error") {
            window.location.href = GridSorter.ErrorUrl;
        }

        GridSorter.AssignSortEvents();

        if (GridSorter.CallBackInjectedFunc) {
            GridSorter.CallBackInjectedFunc();
        }
    },

    UpdateGrid: function (e) {
        debugger;
        $("#__BlockScreen__").css('display', 'block');

        var url = $(e).attr('href');

        var grid = $(e).parents('#GridContainer');

        grid.load(url + " #TableContainer", GridSorter.UpdateGridCallBack);

        return false;
    }
};

(function ($) {

    $.toggler = $.toggler || {};

    var options = {
        ID: "#DaySelector",
        DataAttributeName: "day"
    };

    var anchors = {};

    var clickedData = {};

    var clickedElement = {};

    var array = {};

    var length = {};

    var valueField = {};

    var onload = function (opt) {
        $.extend(options, opt);
        valueField = $("#" + options.ID)[0];
        anchors = $(this.selector + " ul a");
        array = getArray();
        length = array.length;
        if (anchors.length > 0) {
            initItems();
        }
    };

    var bindClickEvent = function (object) {
        $(object).click(function () {
            toggle(object);
        });
    };

    var initItems = function () {
        $.each(anchors, function (itemIndex, object) {
            bindClickEvent(object);
            var data = $(object).data(options.DataAttributeName);
            if (exists(data)) {
                $(object).addClass("Selected");
            }
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
        debugger;
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
    };

    var remove = function (data) {
        clickedElement.removeClass("Selected");

        var oldArr = array;

        var newArr = $.grep(oldArr, function (value) {
            return value !== data;
        });

        var newValue = newArr.join(';');

        setValue(newValue);
    };

    $.fn.Toggler = onload;

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