
function SaveEvent() {
    $('form').submit(function (e) {
        e.preventDefault();
        var returnUrl = $('[name="returnUrl"]');

        var param = {};
        var WebUser = {};
        var ID = $('[Name="ID"]').val();
        var Name = $('[Name="Name"]').val();
        var DeviceTypeID = $('[name="DeviceTypeID"]').val();
        var ManufacturerID = $('[name="ManufacturerID"]').val();
        var SupplierID = $('[name="SupplierID"]').val();
        var ProductDate = $('[name="ProductDate"]').val();
        var MaintainDate = $('[name="MaintainDate"]').val();
        var Note = $('textarea[name="Note"]').val();
        var DeviceState = $('[name="DeviceState"]').val();

        param.Name = Name;
        param.DeviceTypeID = DeviceTypeID;
        param.ManufacturerID = ManufacturerID;
        param.SupplierID = SupplierID;
        param.ProductDate = ProductDate;
        param.MaintainDate = MaintainDate;
        param.Note = Note;
        param.ID = ID;
        param.DeviceState = DeviceState;

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

