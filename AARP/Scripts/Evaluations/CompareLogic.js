CompareLogic = {
    init: function () {

        //click on element
        $('.standing-item').on('click', function () {
            $(this).toggleClass('is_active');

            var isChecked = $(this).hasClass('is_active');

            $(this).attr('compare', isChecked);
        });

        //compare click
        $('.compare').on('click', function (e) {
            var compareItems = $(".standing-item[compare='true']");
            console.log(compareItems);

            if (compareItems.length > 1) {
                console.log('sorting');
                CompareLogic.sortInterviewrs();
            }
            else {
                alert('Please, select at least two interviewers.');
            }

            e.stopPropagation();
        });

        //case when redirect from search box
        var compareItems = jQuery.getParameterByName("comparsing");
        if (compareItems != null) {
            var compareArray = compareItems.split(";");
            CompareLogic.markCompareAndSort(compareArray);
        }
       
    },
    //sorting with compare
    sortInterviewrs: function () {
        $('.interviewers').append(
            $('.standing-item').sort(function (a, b) {
                var blockA = $(a).attr('compare');
                var blockB = $(b).attr('compare');
                console.log(blockA, blockB);
                return (blockA < blockB) ? 1 : (blockA > blockB) ? -1 : 0;
            }));
    },
    markCompareAndSort: function (compareArray) {
        debugger;
        if (compareArray.length > 1) {
            compareArray.forEach(function (item) {
                var selector = ".standing-item[interviewer-id='" + item + "']";
                var compareItem = $(selector);
                compareItem.addClass('is_active');
                compareItem.attr('compare', true);
            });
            CompareLogic.sortInterviewrs();
        }
    }
}