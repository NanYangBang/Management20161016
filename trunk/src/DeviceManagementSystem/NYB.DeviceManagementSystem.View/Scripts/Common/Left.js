function LeftHighLeight() {
    var actionname = $('.sub-content div').first().attr('leftname');
    var navTop = $('.nav-left ul li[leftname="' + actionname + '"]');
    navTop.find('a').css('background-color', '#F1E6E6');

    var otherAction = $('.nav-left ul li[leftname!="' + actionname + '"]');
    otherAction.find('a').css('background-color', 'white');
}

$(document).ready(function () {
    LeftHighLeight();
});
