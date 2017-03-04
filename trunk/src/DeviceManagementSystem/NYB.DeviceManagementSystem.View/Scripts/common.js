function InitializeFrameSize() {
    var nav_left = $('.nav-left');
    var nav_left_handle = $('.nav-left-handle');
    var sub_content = $('.sub-content');
    var content = sub_content.parent();
    var nav_top = $('.nav-top');
    var view_path = $('.view-path');
    var header = $('.header');
    var footer = $('.footer');
    var is_nav_left_handler_clicked = true;
    var nav_left_max_width = Number(nav_left.attr('maxWidth'));
    var nav_left_width = nav_left_max_width;
    var nav_top_height = nav_top.length == 0 ? 0 : nav_top.height();

    $(window).resize(function () {
        setTimeout(resize_content_area, 1);
    });

    resize_content_area();

    setTimeout(function () { $('body').css('zoom', 1); }, 1);

    function resize_content_area() {
        var h = $('body').get(0).clientHeight - header.height() - view_path.outerHeight() - nav_top_height - footer.height() - (content.outerHeight() - content.height());
        content.height(h);
        resize_nav_left();
        resize_subcontent_area();
        content.css('visibility', 'visible');
        if (content.width() > 1200) {
            $('table .nowrap:first').addClass('reset-width');
        } else {
            $('table .reset-width').removeClass('reset-width');
        }
    }

    function resize_subcontent_area() {
        if (nav_left.length == 0) {
            sub_content.css({ width: '100%' });
            return true;
        }

        sub_content.css({ width: content.get(0).clientWidth - nav_left.get(0).clientWidth - nav_left_handle.width(), marginLeft: nav_left.width() + nav_left_handle.width() });

    }

    function resize_nav_left() {
        if (nav_left.length == 0) { return true; }
        if (is_nav_left_handler_clicked) {
            is_nav_left_handler_clicked = false;
            var is_collapse = nav_left.attr('collapse') == 'true';
            nav_left.attr('collapse', !is_collapse);
            nav_left_width = is_collapse === true ? nav_left_max_width : 0;
            nav_left_handle.toggleClass('nav-left-handle-collapse');
        }

        nav_left.css({
            //			top: content.offset().top,
            height: content.get(0).clientHeight,
            overflow: nav_left_width === 0 ? 'hidden' : '',
            width: nav_left_width
        });

        nav_left_handle.css({
            //			top: content.offset().top,
            height: content.get(0).clientHeight,
            left: nav_left_width
        });

        if (typeof (navLeftResizeComplete) == 'function') {
            navLeftResizeComplete();
        }
    }

    nav_left_handle.click(function () {
        is_nav_left_handler_clicked = true;
        resize_nav_left();
        resize_subcontent_area();
    });
}

//导航条下拉菜单
function select_list() {
    $(".nav-anchor").click(function () {
        $("#dropdown_list").show();
    }).mouseleave(function () {
        $("#dropdown_list").hide();
    });
}


//顶部导航条：高亮当前功能
function HighlightNavTopItem(fid) {
    $('.nav-top [fid]').each(function (i, li) {
        if ($(li).attr('fid') == fid) {
            $(li).addClass('current');
        }
    });
}

//生成当前页面的路径指南
function GenerateFunctionPath(fid, tree) {
    var pathArray = [];
    var currentPage = $(tree).find('#' + fid);
    var currentPageTitle = currentPage.attr('title');

    if (currentPage.length == 0) {
        //		if (location.hostname === 'localhost') { alert('Current Page is not registered'); }
        //		location.href = ResourceUrl.HomePage;
        return;
    }

    currentPage.parents('li').find('>a').each(function (i, a) {
        var url = $(a).attr('href') || '';
        if ($(a).attr('self') !== undefined) {
            pathArray.push(generatePathLink($(a).text(), url + $.query.set('preventCache', new Date().getTime()).toString()));
        } else if ($(a).attr('parent') !== undefined) {
            pathArray.push(generatePathLink($(a).text(), url + $.query.toString()));
        } else if ($(a).attr('returnUrl') !== undefined) {
            var currentReturnUrl = $.query.get('returnUrl');
            if (currentReturnUrl === true || currentReturnUrl == '') {
                currentReturnUrl = location.href;
            }
            pathArray.push(generatePathLink($(a).text(), currentReturnUrl));
        } else if ($(a).attr('dynamic') !== undefined) {
            var dynamicMark = $('a[dynamic]');
            if (dynamicMark.length == 0) {
                pathArray.push(generatePathLink('Dynamic Text', '#'));
            } else {
                var dynamicTemplate = $(a).text();
                if (dynamicTemplate.indexOf('{0}') == -1) {
                    dynamicTemplate = '{0}';
                }
                var dynamicText = String.format(dynamicTemplate, dynamicMark.text());
                $(a).text(dynamicText);
                pathArray.push(generatePathLink(dynamicText, dynamicMark.attr('href') || a.href));
            }
        } else {
            pathArray.push(generatePathLink($(a).text(), url));
        }

        document.title = currentPageTitle == undefined ? currentPage.text() : currentPageTitle;
    });

    function generatePathLink(name, url) {
        return String.format('<li><a href="{0}">{1}</a></li>', url, name);
    }
    $('.view-path').html(pathArray.reverse().join('<b>></b>'));

    // Resize Silverlight Object
    var silverlightObjectForm = $('form:has(object[type*="silverlight"])');
    if (silverlightObjectForm.length > 0) {
        $(window).resize(function () {
            $('.sub-content').css('overflow-y', 'hidden');
            var objectHeight = $('.sub-content').innerHeight();
            silverlightObjectForm.height(objectHeight);
        });
        $(window).resize();
    }
}

function GenerateMainFunctionMenu(fid, tree) {
    var functionNameArray = [];
    var currentPage = $(tree).find('#' + fid);
    if (currentPage.length == 0) {
        //		alert('Current Page is not registered');
        return;
    }
    var currentFirstLevelSelector = String.format('[level="1"]:has("#{0}")>li>a', fid);

    var currentMainFunctionName = $(tree).find(currentFirstLevelSelector).text();

    var allFirstLevelSelector = String.format('[level="1"]>li>a');

    functionNameArray.push('<ul class="main-view">');

    functionNameArray.push('<li>');

    functionNameArray.push('<span title="查看主要功能菜单">' + currentMainFunctionName + '</span>');

    functionNameArray.push('<ul class="main-view-link-list">');

    $(tree).find(allFirstLevelSelector).each(function (i, a) {
        functionNameArray.push(generateMainFunctionLink($(a).text(), $(a).attr('href')));
    });

    functionNameArray.push('</ul>');

    //	functionNameArray.push('<iframe frameborder="0" tabindex="-1" id="gbs" src="javascript:void(0)" style="width: 148px; height: 252px; left: 0px; right: auto;"></iframe>');

    functionNameArray.push('</li>');

    functionNameArray.push('</ul>');


    function generateMainFunctionLink(name, url) {
        return String.format('<li><a href="{0}">{1}</a></li>', url, name);
    }

    $('.nav-top').html(functionNameArray.join('')).find('.main-view span').click(toggleMenuList);

    function containsGEPlugin() {
        return $('[id*=_idlglue_pluginDiv]').length > 0;
    }

    function containsNWControl() {
        return $('object[id^="NWControl"]').length > 0;
    }

    function toggleMenuList() {
        var menuList = $('.main-view-link-list');

        if (!containsGEPlugin() && !containsNWControl()) {
            menuList.slideToggle('fast');
            return true;
        }
        menuList.toggle();
        var shim = $('.shim');

        if (shim.length == 0) {
            shim = $('<iframe frameborder="0" tabindex="-1" class="shim"></iframe>');
            menuList.parent().append(shim);
        } else {
            shim.remove();
            return;
        }
        shim.css({
            position: 'absolute',
            left: menuList.position().left,
            top: menuList.position().top,
            width: menuList.outerWidth(),
            height: menuList.outerHeight()
        });
    }

    //hide menu while click other area
    $(document).click(function (e) {
        var menuList = $('.main-view-link-list');
        var isMenuItself = $(e.target).next('.main-view-link-list').length > 0 || $(e.target).parents('.main-view-link-list').length > 0;
        if (!isMenuItself) {
            $('.shim').remove();
            if (containsGEPlugin()) {
                menuList.slideUp('fast');
            } else {
                menuList.hide();
            }
        }
    });
}

function GenerateSecondLevelFunctionMenu(fid, tree) {

    var childFunctionArray = [];
    var currentPage = $(tree).find('#' + fid);
    if (currentPage.length == 0) {
        //		alert('Current Page is not registered');
        return;
    }
    //		childFunctionArray.push('<ul class="child-view">');

    var currentSecondLevelSelector = String.format('[level="2"]:has("#{0}")>li>a', fid);
    var allSecondLevelLinks = $(tree).find(currentSecondLevelSelector);
    allSecondLevelLinks.each(function (i, a) {
        if ($(a).attr('id') == fid || $(a).parent().find('#' + fid).length > 0) {
            childFunctionArray.push(String.format('<li class="nav-selected"><span>{0}</span></li>', $(a).text()));
        }
        else {
            childFunctionArray.push(generateChildFunctionLink($(a).text(), $(a).attr('href')));
        }
    });

    //		childFunctionArray.push('</ul>');
    $('.main-view').append((childFunctionArray.join(''))).find('>li').last().find('>a').addClass('last');


    function generateChildFunctionLink(name, url) {
        return String.format('<li><a href="{0}" {2}>{1}</a></li>', url, name);
    }
}

function FindMarkAndGenerateNavigateElements() {
    var markName = 'viewIndex';
    var mark = $(String.format('[{0}]', markName)).last();
    if (mark.length == 0) {
        return false;
    }
    var fid = mark.attr(markName);
    $.noCacheGet(ResourceUrl.ViewIndex, { controllerName: ResourceUrl.CurrentControllerName }, function (tree) {
        GenerateFunctionPath(fid, tree);
        GenerateMainFunctionMenu(fid, tree);
        GenerateSecondLevelFunctionMenu(fid, tree);
    });
}

$.extend({
    noCacheGet: function (url, data, callback, type) {
        if (jQuery.isFunction(data)) {
            type = type || callback;
            callback = data;
            data = undefined;
        }

        return jQuery.ajax({
            type: "GET",
            url: url,
            data: data,
            cache: false,
            success: callback,
            dataType: type,
            contentType: "application/x-www-form-urlencoded; charset=utf-8"
        });
    }
});

function BindAllDisabledButton() {
    $('.disabled-button').click(function () {
        alert(this.title);
    });
}

function BindNonNumberBlocker(selector, allowNegative, allowZero) {
    if (allowNegative == undefined) { allowNegative = true; }
    if (allowZero == undefined) { allowZero = false; }
    $(selector).keydown(function (e) {
        // 9: Tab; 8 : Backspace; 16: Shift; 46: Delete; 33~40: Left, Right, Up, Down, Home, End, PageUp, PageDown; 96~105: NumPad(0-9); 48~57: 0-9
        if (e.keyCode == 8 || e.keyCode == 9 || e.keyCode == 16 || e.keyCode == 46
		|| (e.keyCode >= 33 && e.keyCode <= 40)
		|| (e.keyCode > 96 && e.keyCode <= 105)
		|| (e.keyCode > 48 && e.keyCode <= 57 && e.shiftKey == false)
            //|| (allowNegative && $(this).val() == '' && (e.keyCode == 189 || e.keyCode == 109))
		|| (allowZero && e.keyCode == 48 && e.shiftKey == false && $(this).val() == '')
		|| (e.keyCode == 48 && e.shiftKey == false && $(this).val() != '') // not blank, always allow zero
		|| (e.keyCode == 96 && e.shiftKey == false && $(this).val() != '')
		) { } else { e.preventDefault(); }
    });
}

function BindDatePickerTriggerEvent() {
    return;

    $('.ui-datepicker-trigger').click(AppendClearButtonToDatePicker);

    function AppendClearButtonToDatePicker() {
        var picker = $('#ui-datepicker-div');
        var panel = picker.find('.ui-datepicker-buttonpane');
        var btnClearDate = panel.find('#clearDateField');

        if (btnClearDate.length > 0) { return false; }

        btnClearDate = $('<button type="button" id="clearDateField" class="ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all">清空</button>');
        panel.append(btnClearDate);

        btnClearDate.click(function () {
            var inputID = /'(#.+?)'/.exec(picker.find('a:first').attr('onclick'))[1];
            $(inputID).val('');
        });
    }
}

function DatePickerZIndexPatch() {
    $('.ui-datepicker-trigger').click(function () {
        $('#ui-datepicker-div').css('z-index', 2);
    });
}

function ApplyStyleToCommonTable() {
    if ($.browser.msie) {
        $('table.table-common > tbody > tr:even').addClass('even');
    }
}

function LimitTextBoxMaxLength() {
    var allTextBoxes = $('input[type="text"]:not([maxlength])');
    allTextBoxes.attr('maxlength', 50);
    allTextAreas = $('textarea:not([maxlength])');
    allTextAreas.attr('maxlength', 200);

}

$(document).ready(function () {
    ApplyStyleToCommonTable();
    //LimitTextBoxMaxLength();
    hideDeleteButton();
    BindKeyDownEventToSearchBox();
    InitializeListFilter();
});

function InitializeListFilter() {
    var pnlFilter = $('.list-filter');
    if (pnlFilter.length == 0) return;
    var allKeys = $.query.keys;

    for (var key in allKeys) {
        if (key.toLowerCase() == 'pageindex') { continue; }
        var nodes = key.split('.');
        var lastNode = nodes[nodes.length - 1];
        var input = pnlFilter.find(String.format('[name="{0}"]', lastNode));

        var value = allKeys[key];
        if (value === true || value === '') {
            // ignore
        } else {
            if (input.length > 0) {
                //change
                input.val(value);
            } else {
                //append
                pnlFilter.prepend(String.format('<input type="hidden" name="{0}" value="{1}"/>', key, allKeys[key]));
            }
        }
    }

    pnlFilter.find('select').change(function () {
        pnlFilter.submit();
    });
}

//全选功能。
function BindTableCheckBoxEvent() {
    var btnCheckAll = $('th:first-child :checkbox');
    var enabledCheckboxesSelector = 'table td:first-child input:checkbox:not(:disabled)';
    var enabledCheckboxes = $(enabledCheckboxesSelector);

    btnCheckAll.click(function () {
        $(enabledCheckboxesSelector).attr("checked", this.checked);
    });

    enabledCheckboxes.click(function () {
        var enabledCheckboxes = $(enabledCheckboxesSelector);
        btnCheckAll.prop('checked', enabledCheckboxes.filter(':checked').length == enabledCheckboxes.length);
    });
}

function hideDeleteButton() {
    var table = $('table');
    if (table.length != 0) {
        var btn = $('input[type=button]').filter('[value="删除"]');
        if (table.find('tr td :checkbox').length > 0) {
            if (table.find('tr td :checkbox:enabled').length === 0) {
                btn.val() == "删除" ? btn.show().remove() : "";
            }
        } else {
            btn.hide();
        }
    }
}

function BindKeyDownEventToSearchBox() {
    var btnSearch = $(':submit[value="搜索"]');
    if (btnSearch.length > 0) {
        btnSearch.parent().find(':text').keydown(startSearch);
    }

    function startSearch(e) {
        if (e.keyCode == 13) {
            btnSearch.click();
        }
    }
}

function AddEditorLeavingPrompt() {
    var pnlEditor = $('.common-details-panel');
    var pnlOperation = $('.common-details-operation-panel');
    if (pnlEditor.length == 0) { return; }

    initializeEditor();

    window.onbeforeunload = function () {
        checkEditor();
        if (window.LeavingPrompt === true && !window.Submitting && !window.Review) {
            return '页面中的表单已经被修改，是否放弃保存立即离开页面？';
        } else {
        }
    };


    function initializeEditor() {
        pnlEditor.find(':input').each(function (i, input) {
            $(input).attr('hash', getValue(input).hashCode());
        });

        pnlOperation.find(':input').click(function () {
            window.Submitting = true;
        });
        pnlEditor.find('.audit-button').click(function () {
            window.Review = true;
        });
    }

    function getValue(input) {
        var value = $(input).prop('type') == 'checkbox' ? $(input).prop('checked') : $(input).val();
        return String(value);
    }

    function checkEditor() {
        window.LeavingPrompt = false;
        pnlEditor.find(':input[hash]').each(function (i, input) {
            if (getValue(input).hashCode().toString() != $(input).attr('hash')) {
                window.LeavingPrompt = true;
                return false;
            }
        });
    }
}

function NotShowDelButton() {
    var tableCommon = $('table.table-common');
    if (tableCommon.find('tbody tr :checkbox:not(":disabled")').length == 0) {
        $('input:button').filter('[value="删除"]').hide();
    }
}

function WhenDeletePrompt() {
    if ($('.wrap .delWaitPrompt').length == 0) {
        $('.wrap').append('<div style="" class="waitShowWhenDel"><p class="waitMessageWhendel">正在删除请稍候……<p></div>)');
        $(".waitShowWhenDel").fadeTo(0, 0.5);
    }
}

function ReplaceAllDisableTagToLabel(params) {

    var selectorReplaceArea = '*', selectorDeletion = '', fn;

    if (typeof params == 'object') {
        selectorReplaceArea = params.area || selectorReplaceArea;
        selectorDeletion = params.remove || selectorDeletion;
        fn = typeof params.fn == 'function' ? params.fn : null;
    }

    var pnlReplaceArea = $('div.content ' + selectorReplaceArea);

    pnlReplaceArea.find('em.red' + (selectorDeletion == '' ? '' : ', ' + selectorDeletion)).remove();

    var className = 'view-mode';

    pnlReplaceArea.addClass(className).find('select, input, textarea').each(function () {
        var source = $(this);
        var text;

        switch (this.tagName) {
            case 'INPUT':
                switch (this.type) {
                    case 'text':
                        $(this).prop('readonly', true).addClass(className);
                        break;
                    case 'checkbox':
                        break;
                    case 'radio':
                        break;
                    case 'button':
                    case 'submit':
                        $(this).remove();
                        break;
                    default:
                }
                break;
            case 'SELECT':
                text = source.find('option:selected').text();
                source.replaceWith(String.format('<input type="text" readonly class="{1}" value="{0}"></span>', text, className));
                break;
            case 'TEXTAREA':
                $(this).prop('readonly', true).addClass(className);
                if (this.scrollHeight < this.clientHeight) {
                    $(this).css('overflow', 'hidden');
                }
                break;
            default:
        }
    });


    pnlReplaceArea.find('a:contains(返回)').addClass(className + ' inline-block button').addClass($('.right-panel').length == 0 ? '' : 'right');

    pnlReplaceArea.find('a:not([href]):not([dynamic])').remove();

    var pnlLeft = $('div.left-panel');
    if (pnlLeft.find('.last-modified-time').text().indexOf('20') == -1) {
        pnlLeft.find('input:text, textarea').val('');
        pnlLeft.find('dd.prefix > em').remove();
    }

    fn && fn();

}

function CaptureFormResult(formSelector, fn) {
    var collectorId = 'formResultCollector';

    $(formSelector).prop('target', collectorId);

    var iframe = $(String.format('<iframe name="{0}" id="{0}" class="hidden"/>', collectorId));
    iframe.appendTo($('body')).load(function () {
        var text = $(this).contents().text();
        if (text.length < 2) {
            return;
        } else if (text[0] != '{') {
            alert('Result is not the JSON format!');
            return;
        }
        var result = text.toJson();
        fn(result);
    });
}



function SortAlertRecord() {
    $('.table-common thead tr th a').click(function (e) {
        e.preventDefault();
        var currentSortBy = UrlResource.UrlCurrentSortBy;
        var currentAscending = UrlResource.UrlCurrentAscending;
        var sortBy = $(this).attr('sortBy');
        var ascending = true;
        if (sortBy == currentSortBy) {
            ascending = currentAscending == 'false';
        }
        location.href = $.query.set('sortBy', sortBy).set('ascending', ascending.toString()).toString();
    })
}


function AddTxtAre() {
    var textAreaTD = $('.lengthTd');
    var textareaMode = textAreaTD.find('textarea');
    textareaMode.height(textAreaTD.attr('ht'));
    textareaMode.width(textAreaTD.attr('wt'));
    var operateParent = textAreaTD.parents('tr');
    
    textAreaTD.prev().height(textAreaTD.attr('ht'));
    textAreaTD.parents('fieldset').css('margin-bottom', '20px')

}

