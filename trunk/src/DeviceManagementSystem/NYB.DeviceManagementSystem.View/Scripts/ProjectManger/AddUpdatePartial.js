
function SaveEvent() {
    $('form').submit(function (e) {
        e.preventDefault();
        var returnUrl = $('[name="returnUrl"]');
        
        var param = {};
        var WebUser = {};
        var ID = $('[name="ID"]').val();
        var Name = $('[name="Name"]').val();
        var Note = $('textarea[name="Note"]').val();

        var UserName = $('[name="WebUser.UserName"]').val();
        var LoginName = $('[name="WebUser.LoginName"]').val();
        var Pwd = $('[name="WebUser.Pwd"]').val();
        var TelPhone = $('[name="WebUser.TelPhone"]').val();
        var Address = $('[name="WebUser.Address"]').val();
        var Email = $('[name="WebUser.Email"]').val();

        param.ID = ID;
        param.Name = Name;
        param['WebUser.LoginName'] = LoginName;
        param['WebUser.UserName'] = UserName;
        param['WebUser.Pwd'] = md5(Pwd);
        param['WebUser.TelPhone'] = TelPhone;
        param['WebUser.Address'] = Address;
        param['WebUser.Email'] = Email;
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

