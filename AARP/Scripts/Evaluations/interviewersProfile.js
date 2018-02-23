var interviewerProfile = (function () {

    var currentInterviewerId = null;
    var feedBackTemplate = null;
    var filter = {};
    var dataTable = null;


    function initGrid() {

        // Check if initialized
        if (dataTable) {
            return;
        }

        dataTable = $('#interviews-table').DataTable({
            searching: false,
            //"columnDefs": [
            //    { "orderable": false, "targets": 0 }
            //],
            //"order": [[1, "desc"]],
            responsive: true,
            processing: true,
            serverSide: true,
            deferLoading: 10,
            //"pageLength": 4,
            ajax: {
                type: "POST",
                contentType: "application/json",
                url: '/Evaluations/SelectInterviews',
                data: function (d) {
                    // note: d is created by datatable, the structure of d is the same with DataTableParameters model above
                    
                    d.filter = filter;
                    d.interviewerId = currentInterviewerId;
                    return JSON.stringify(d);
                }
            },
            "columns": [
                {
                    "data": "Date",
                    "render": function (data, type, full, meta) {
                        return meta.settings._iDisplayStart + meta.row + 1 + ". ";
                    }
                },
                { "data": "IntervieweeName" },
                {
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": '<a href="javascript:void(0)">Details<b class="caret caret-up"></b></a>'
                },
                {
                    "data": "Date",
                    "render": function (value) {
                        if (value === null) return "";

                        var pattern = /Date\(([^)]+)\)/;
                        var results = pattern.exec(value);
                        var dt = new Date(parseFloat(results[1]));

                        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                    }
                },
                { "data": "InterviewStage" },
                { "data": "Seniority" },
                { "data": "Technology" },
                { "data": "Status" },
                {
                    "data": "Rating",
                    "render": function (value) {
                        var result = value + '.0 ';

                        for (var i = 0; i < 5; i++) {
                            if (value > i)
                                result += '<i class="glyphicon glyphicon-star"></i>';
                            else result += '<i class="glyphicon glyphicon-star-empty"></i>';
                        }

                        return result;
                    }
                },
            ]
        });

        // Add event listener for opening and closing details
        $('#interviews-table tbody').on('click', 'td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = dataTable.row(tr);
            $(this).find('b').toggleClass('caret-up');

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                row.child(format(row.data())).show();
                tr.addClass('shown');
            }
        });

        $(document).on("click", ".filter-check", function () {
            var type = $(this).attr('f-type') + 'Ids';
            if (!filter[type])
                filter[type] = [];

            if ($(this).is(':checked'))
                filter[type].push($(this).val());
            else {
                var index = filter[type].indexOf($(this).val());
                filter[type].splice(index, 1);
            }

            dataTable.ajax.reload();
        });
    }

    function format(d) {
        // `d` is the original data object for the row

        var html = $(feedBackTemplate);
        console.log(html);

        html.find('[value="' + d.Attitude + '"]').attr('checked', true);
        if (d.TestTask != null) {
            html.find('[value="' + (d.TestTask ? "yes" : "no") + '"]').attr('checked', true);
        }

        html.find('[value="' + d.Difficulty + '"]').attr('checked', true);

        html.find('[value="l' + d.Length + '"]').attr('checked', true);
        html.find('[name="feedback"]').text(d.Feedback);

        return html.html();
    }

    function reload(interviewerId) {
        currentInterviewerId = interviewerId;
        $.ajax({
            url: '/Evaluations/GetInterviewsFilter',
            data: { interviewerId: interviewerId },
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#general-stats').html(data);
            },
        });

        $('#interviews-table').DataTable().draw();
    }

    function setFeedbackTemplate(template) {
        feedBackTemplate = template;
    }

    return {
        initGrid: initGrid,
        reload: reload,
        setFeedbackTemplate: setFeedbackTemplate
    }
    
})();
