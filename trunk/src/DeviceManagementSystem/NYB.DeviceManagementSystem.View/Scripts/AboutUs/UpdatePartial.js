
function SaveEvent() {
    $('form').submit(function (e) {
        e.preventDefault();
        var returnUrl = $('[name="returnUrl"]');

        var param = {};
        var CompanyContact = $('[name="CompanyContact"]').val();
        var CompanyDescribe = $('[name="CompanyDescribe"]').val();
        var CompanyName = $('[name="CompanyName"]').val();
        var ID = $('[name="ID"]').val();

        param.ID = ID;
        param.CompanyContact = CompanyContact;
        param.CompanyDescribe = CompanyDescribe;
        param.CompanyName = CompanyName;

        if ($('span.errorMessage').length == 0 && $('.field-validation-error').length == 0) {
            AjaxEvent(Resource.UrlEdit, param);
        }
    });
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

