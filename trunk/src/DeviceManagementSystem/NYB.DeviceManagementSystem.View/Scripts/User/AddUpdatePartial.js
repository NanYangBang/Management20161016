
function SaveEvent() {
    $('form').submit(function (e) {
        e.preventDefault();
        var returnUrl = $('[name="returnUrl"]');

        var param = {};
        var UserName = $('[name="UserName"]').val();
        var LoginName = $('[name="LoginName"]').val();
        var Pwd = $('[name="Pwd"]').val();
        var Address = $('[name="Address"]').val();
        var TelPhone = $('[name="TelPhone"]').val();
        var RoleName = $('[name="Role"]').val();
        var ID= $('[name="ID"]').val();

        param.ID = ID;
        param.UserName = UserName;
        param.LoginName = LoginName;
        param.Pwd = Pwd;
        param.Address = Address;
        param.TelPhone = TelPhone;
        param.Role = RoleName;

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

