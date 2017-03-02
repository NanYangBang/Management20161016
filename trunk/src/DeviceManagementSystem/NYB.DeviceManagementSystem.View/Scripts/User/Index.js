

function AddUpdateEvent() {
    $('#addUser').click(function () {
        window.location = window.UrlResource.UrlAddAction;
    })

}

function DataDel(id, url) {
    if (window.confirm("是否删除？")) {
        $.post(url, { id: id }, function (result) {
            if (result.Code == 0) {
                window.location.reload();
            } else {
                alert(result.Msg);
            }
        })
    }
}

function SearchInfoFocus(prompt) {
    var domeSearInfoText = $('#searchForm input[type="text"]');

    var firstVal = domeSearInfoText.val();
    if (firstVal == '') {
        domeSearInfoText.val(prompt);
        domeSearInfoText.css('color', '#ccc');
    }

    domeSearInfoText.focus(function () {
        var thisVal = $(this).val();

        if (prompt == thisVal) {
            $(this).val('');
            domeSearInfoText.css('color', 'black');
        }

    })

    domeSearInfoText.blur(function () {
        var thisVal = $(this).val();

        if (thisVal == '' || thisVal == prompt) {
            $(this).val(prompt);
            domeSearInfoText.css('color', '#ccc');
        }
    })

}


