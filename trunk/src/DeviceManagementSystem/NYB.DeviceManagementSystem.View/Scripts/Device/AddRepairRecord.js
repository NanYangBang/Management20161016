
function SaveEvent() {
    $('form').submit(function (e) {
        e.preventDefault();
        var returnUrl = window.Resource.returnUrl;

        var param = {};
        var WebUser = {};
        var DeviceID = $('[Name="DeviceID"]').val();
        var DeviceName = $('[Name="DeviceID"]').val();
        var Operator = $('[Name="Operator"]').val();
        var RepairDate = $('[Name="RepairDate"]').val();
        var Note = $('textarea[Name="Note"]').val();
        var ID = $('[Name="RepairRecordID"]').val();

        param.DeviceID = DeviceID;
        param.DeviceName = DeviceName;
        param.Operator = Operator;
        param.RepairDate = RepairDate;
        param.Note = Note;
        param.ID = ID;

        if ($('span.errorMessage').length == 0 && $('.field-validation-error').length == 0) {
            if (Resource.Action == 'Add') {
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

