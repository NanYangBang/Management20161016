
function SaveEvent() {
    $('form').submit(function (e) {
        e.preventDefault();
        var returnUrl = $('[name="returnUrl"]');

        var param = {};
        var Name = $('[name="Name"]').val();
        var LogoName = $('[name="LogoName"]').val();
        var Pwd = $('[name="Pwd"]').val();
        var Note = $('[name="Note"]').val();

        param.Name = Name;
        param.LogoName = LogoName;
        param.Pwd = Pwd;
        param.Note = Note;

        if ($('span.errorMessage').length == 0 && $('.field-validation-error').length == 0) {
            if (Resource.UrlAction == 'Add') {
                AjaxEvent(Resource.UrlAdd, param);
            } else {
                AjaxEvent(Resource.UrlEdit, param);
            }
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

