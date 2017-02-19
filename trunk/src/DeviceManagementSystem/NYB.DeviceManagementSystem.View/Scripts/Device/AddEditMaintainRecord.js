
function uploadProof() {
    var action = Resource.UrlAddFile;
    if (Resource.IsAddRecord == 'False') {
        action = Resource.UrlEditFile;
    }
    $('#ssi-upload').uploadify({
        uploader: action,
        swf: Resource.UrlSwf,
        cancelImage: Resource.UrlCancelImage,
        buttonText: "浏览",
        auto: false,
        width: 60,
        multi: true,
        //postData: { token: Resource.Auth },
        //checkExisting: false,
        fileTypeDesc: '*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.jpg;*.jpeg;*.bmp;*.png',
        //removeCompleted: true,
        fileTypeExts: '*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.jpg;*.jpeg;*.bmp;*.png',
        fileSizeLimit: 10240,
        onQueueComplete: function () {
            window.location = Resource.ReturnUrl;
        },
        onError: function (event, ID, fileObj, errorObj) {
            alert("上传失败，请稍后再试！");
        },
        onUploadError: function (file, data) {
        },
        onUploadSuccess: function (file, data, response) {

        }
    });
}

function loadUplodify() {
    if (Resource.IsAddRecord == 'False') {
        $('#markerFile li').each(function (i, li) {
            var filePath = $(li).attr('filePath');
            var fileName = $(li).attr('fileName');
            var fileID = $(li).attr('fileID');
            var viewPicturePath = String.format("{0}?fileName={1}&filePath={2}", Resource.UrlViewPicture, fileName, filePath);
            var target = String.format("{0}?fileName={1}&filePath={2}", Resource.UrlDownLoad, fileName, filePath);
            $('.uploadifyQueue').append(String.format('<div class="uploadifyQueueItem"><div class="cancel"><div id="{3}" style="cursor:pointer" fileName="{1}" filePath="{5}" class="btndownload"><a href="{0}" target="_top"><img src="{6}" /></a></div><div id="{3}" style="cursor:pointer" class="btnDel"><img src="{2}" /></div></div><span class="fileName"><a href="#" target="_blank" filePath="{5}" id="DownLoadFile"  class="upload_fileName" >{1}</a></span></div>', target, fileName, Resource.UrlCancelImage, fileID, viewPicturePath, filePath, Resource.ImgDownLoad));
        })
    }

    $(document).on('click', '#DownLoadFile', function (e) {
        //var picktureDiv = $('#pictureView');
        //picktureDiv.appendTo($('body'));
        //var pagecontainer = $('.warp');
        //picktureDiv.css('width', '100%').css('height', '100%').css('background-color', '#ccc').css('visibility', 'visible');
        e.preventDefault();
        var filePath = $(this).attr('filePath');
        $.get(Resource.UrlViewPicture, { filePath: filePath }, function () {

        })

    });

    //$(document).on('click', '.btndownload', function (e) {
    //    //var picktureDiv = $('#pictureView');
    //    //picktureDiv.appendTo($('body'));
    //    //var pagecontainer = $('.warp');
    //    //picktureDiv.css('width', '100%').css('height', '100%').css('background-color', '#ccc').css('visibility', 'visible');
    //    e.preventDefault();
    //    var filePath = $(this).attr('filePath');
    //    var fileName = $(this).attr('fileName');
    //    $.get(Resource.UrlDownLoad, { filePath: filePath, fileName: fileName }, function () {

    //    })
    //});

    $(document).on('click', '.btnDel', function () {
        //console.log(this);
        $(this).parents('.uploadifyQueueItem').remove();
        Resource.delID.push($(this).attr('id'));
    })

    if (Resource.IsViewRecord == 'True') {
        $('div.uploadify').remove();
    }

    $('#btnSave').click(function (e) {
        e.preventDefault();
        var errorLength = $('.field-validation-error').length;
        if (errorLength == 0) {
            var webEntity = {};
            var MaintainRecordID = $('[name="MaintainRecordID"]').val();
            webEntity['webMaintainRecord.ID'] = MaintainRecordID;
            webEntity['webMaintainRecord.DeviceID'] = $('[name="DeviceID"]').val();
            webEntity['webMaintainRecord.Operator'] = $('#Operator').val();
            webEntity['webMaintainRecord.MaintainDate'] = $('#MaintainDate').val();
            webEntity['webMaintainRecord.Note'] = $('#Note').val();

            for (var i = 0; i < Resource.delID.length; i++) {
                webEntity['delIDList[' + i + ']'] = Resource.delID[i];
            }

            var fileLenth = $('div.uploadifyProgress').length;
            var handleAction;
            if (Resource.IsAddRecord == 'False') {
                webEntity.reviewID = -1;
                handleAction = Resource.UrlEdit;
                SaveAjax(handleAction, webEntity, fileLenth, MaintainRecordID);
            } else {
                handleAction = Resource.UrlAdd;
                SaveAjax(handleAction, webEntity, fileLenth, MaintainRecordID);
            }
        }
    });
}

function SaveAjax(handleAction, webEntity, fileLenth, materialID) {
    $.post(handleAction, webEntity, function (data) {
        var id;
        if (data.Code == 0) {
            if (fileLenth > 0) {
                if (Resource.IsAddRecord == 'True') {
                    id = data.Data;
                } else {
                    id = materialID;
                }
                SaveUpload(handleAction, id);
            } else {
                window.location.href = Resource.ReturnUrl;
            }
        } else {
            alert(data.Msg);
        }
    });
}

function SaveUpload(handleAction, id) {
    var upload = $('#ssi-upload');
    upload.uploadifySettings('postData', { maintainRecordID: id });
    upload.uploadifyUpload('*');
}


















