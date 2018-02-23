jQuery.extend({
    getValues: function (url, params) {
        var result = null;
        $.ajax({
            url: url,
            type: 'get',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: params,
            success: function (data) {
                result = data;
            }
        });
        return result;
    },
    getParameterByName: function (name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
});

$('#toggleInterview').click(function () {
    $(this).find('.servicedrop').toggleClass('fa-arrow-up fa-arrow-down');
});

$('.menu .dropdown').mouseenter(function () {
    $(this).addClass('open');
});

$('.menu .dropdown').mouseleave(function () {
    $(this).removeClass('open');
});

