
function SaveEvent() {
    $('form').submit(function (e) {
        e.preventDefault();
        var returnUrl = $('[name="returnUrl"]');

        var param = {};
        var Pwd = $('[name="Pwd"]').val();
        var ID = $('[name="ID"]').val();

        param.userID = ID;
        param.newPassword = Pwd;

        if ($('span.errorMessage').length == 0 && $('.field-validation-error').length == 0) {
            AjaxEvent(Resource.UrlEdit, param);
        }
    });
    $('select').change(function () {
        $(this).parent().find('.errorMessage').remove();
    })
}

function AjaxEvent(url, param) {
    $.post(url, param, function (data) {
        //var result = Json.parse(data);
        if (data.Code == '0') {
            window.location.href = Resource.ReturnUrl;
        } else {
            window.alert(data.Msg);
        }
    });
}

