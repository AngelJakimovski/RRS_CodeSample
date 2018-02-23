var interviewsReport = (function () {

    var filter = {};
    var dataTable = null;


    function initGrid() {

        // Check if initialized
        if (dataTable) {
            return;
        }

        dataTable = $('#reports-table').DataTable({
            searching: false,
            //"columnDefs": [
            //    { "orderable": false, "targets": 0 }
            //],
            //"order": [[1, "desc"]],
            responsive: true,
            //processing: true,
            serverSide: true,
            //"pageLength": 4,
            ajax: {
                type: "POST",
                contentType: "application/json",
                url: '/Evaluations/SelectReportItems',
                data: function (d) {
                    // note: d is created by datatable, the structure of d is the same with DataTableParameters model above

                    d.filter = filter;
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
                { "data": "Email" },
                { "data": "InterviewerName" },
                { "data": "FeedbackStatus" },
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
                
                { "data": "Stage" },
                { "data": "Seniority" },
                {
                    "data": "Link",
                    "render": function (value) {
                        if(value)
                            return '<a href="' + value + '" target="_blank">' + value + '</a>';
                        return '';
                    }
                },
            ]
        });
    }

    return {
        initGrid: initGrid,
    }

})();
