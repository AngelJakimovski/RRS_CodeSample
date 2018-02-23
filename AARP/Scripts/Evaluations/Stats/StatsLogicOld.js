var interviewerIdArr = new Array();
var interviewerNameArr = new Array();

function setValue(interviewer) {
    var interviewerId = interviewer.split('|')[0];
    var interviewerName = interviewer.split('|')[1];
    if ($.inArray(interviewerId, interviewerIdArr) != -1) {
        interviewerIdArr = jQuery.grep(interviewerIdArr, function (value) {
            return value != interviewerId;
        });

        interviewerNameArr = jQuery.grep(interviewerNameArr, function (value) {
            return value != interviewerName;
        });

    } else {
        interviewerIdArr.push(interviewerId);
        interviewerNameArr.push(interviewerName);
    }

    if (interviewerIdArr.length > 1) {
        $('#btnCompareInterview').show();
        $('#btnSearchInterview').hide();
    } else {
        $('#btnCompareInterview').hide();
        $('#btnSearchInterview').show();
    }
}
function showResultTable() {
    $("#searchResultPanel").show();
    if (interviewerIdArr.length < 1) {
        $('#resultTable').html('');
        $('#submitTable').html('');
    }
    if ($('#txtSearchField').val().length > 0) {
        loadData();
    }
}

function loadData() {
    $.ajax({
        url: '/Interviewer/GetInterviewer/',
        data: "{ 'key': '" + $('#txtSearchField').val() + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var submitHtml = $('#tempateSubmit').html();

            $.each(data, function (key, value) {
                if ($.inArray(value.Id.toString(), interviewerIdArr) == -1) {
                    var resultHtml = $('#templateRow').html();
                    resultHtml = resultHtml.replace('replaceInterviewerName', value.InterviewerName);
                    resultHtml = resultHtml.replace('replaceInterviewerId', value.Id + "|" + value.InterviewerName);
                    resultHtml = resultHtml.replace('checked', '')
                    $('#resultTable').append(resultHtml);
                }
            });
            if (data.length < 1 && interviewerIdArr.length < 1) {
                var resultHtml = $('#templateNoResult').html();
                $('#resultTable').append(resultHtml);
            }
            else if (interviewerIdArr.length < 1) {
                $('#submitTable').html(submitHtml);
            }
        },
        error: function (response) {
        },
        failure: function (response) {
        }
    });
}

$(document).ready(function () {
    $("body").click(function (e) {
        if (e.target.id == "searchResultPanel" || $(e.target).parents("#searchResultPanel").length || e.target.id == "dropdown" || $(e.target).parents("#dropdown").length) {
            //Nothing
        } else {
            $("#searchResultPanel").hide();
            $('#resultTable').html('');
            $('#submitTable').html('');
            interviewerIdArr = [];
            interviewerNameArr = [];
        }
    });

    $(document).on("click", "#btnCompareInterview", function () {
        var toCompare = [];
        console.log(interviewerIdArr);

        $('.standing-item').each(function (index, elem) {
            var interviwerId = $(elem).attr('interviewer-id');
            var element = $(elem);

            if (interviewerIdArr.indexOf(interviwerId) != -1) {
                element.addClass('is_active');
                element.attr('compare', true);
            }
            else {
                element.removeClass('is_active');
                element.attr('compare', false);
            }
        });

        sortInterviewrs();

        $('#searchResultPanel').hide();
    });

    $(document).on("click", "#btnSearchInterview", function () {
        console.log(interviewerIdArr);

        if (interviewerIdArr.length) {
            interviewerProfile.initGrid();
            interviewerProfile.reload(interviewerIdArr[0]);
            $('#searchResultPanel').hide();

            $('.interviewer-info').addClass('active');
        }
    });

    //$("#btnSearchInterview").click(function () {
    //TODO
    //$.post('/Evaluations/Standings/' + id, function (result) {
    //});
    //});

    $('#txtSearchField').on('input', function (e) {
        $('#btnCompareInterview').hide();
        $('#btnSearchInterview').show();
        $('#resultTable').html('');
        if (interviewerIdArr.length > 0) {
            for (var i = 0; i < interviewerIdArr.length; i++) {
                var resultHtml = $('#templateRow').html();
                resultHtml = resultHtml.replace('replaceInterviewerName', interviewerNameArr[i]);
                resultHtml = resultHtml.replace('replaceInterviewerId', interviewerIdArr[i] + "|" + interviewerNameArr[i]);
                $('#resultTable').append(resultHtml);
                if (i > 0) {
                    $('#btnCompareInterview').show();
                    $('#btnSearchInterview').hide();
                }
            }
        }
        if ($('#txtSearchField').val().length < 1 && interviewerIdArr.length < 1) {
            $('#resultTable').html('');
            $('#submitTable').html('');
        } else {
            loadData();
        }
    });

    $('.standing-item').click(function () {
        $(this).toggleClass('is_active');

        var isChecked = $(this).hasClass('is_active');

        $(this).attr('compare', isChecked);
    });

    $('.compare').click(function (e) {
        var compareItems = $(".standing-item[compare='true']");
        console.log(compareItems);

        if (compareItems.length > 1) {
            console.log('sorting');
            sortInterviewrs();
        }
        else {
            alert('Please, select at least two interviewers.')
        }

        e.stopPropagation();
    });

    $('txtSearchField').on('click', showResultTable());

    function sortInterviewrs() {
        $('.interviewers').append(
            $('.standing-item').sort(function (a, b) {

                var blockA = $(a).attr('compare');
                var blockB = $(b).attr('compare');
                console.log(blockA, blockB)
                return (blockA < blockB) ? 1 : (blockA > blockB) ? -1 : 0;
            }));
    }

});