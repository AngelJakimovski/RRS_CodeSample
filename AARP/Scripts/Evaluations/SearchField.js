SearchField = {
    _properties: {
        interviewersUrl: null,
        interviewerProfileUrl: null,
        standingsUrl : null,
        readyToClose: false,
        selectedItems: [],
        standingsPage: 'False'
    },
    init: function (properties) {
        //extend properties
        $.extend(SearchField._properties, properties);

        //init autocomplete
        $("#txtSearchField")
            .autocomplete({
                source: SearchField.loadData,
                response: SearchField.response,
                open: SearchField.fixPosition,
                select: SearchField.select
            });

        //init render event
        $("#txtSearchField").data('ui-autocomplete')._renderItem = SearchField.renderItem;

        //init close event (to prevent closing)
        $("#txtSearchField").data('ui-autocomplete').close = SearchField.close;

        //closing logic if input empty
        $("#txtSearchField").on('input',
            function () {
                if ($("#txtSearchField").val().length === 0) {
                    SearchField._properties.readyToClose = true;
                    $("#txtSearchField").autocomplete("close");
                } else {
                    SearchField._properties.readyToClose = false;
                }
            });

        //binding search click
        $("#btnSearchInterview").on('click', SearchField.searchClick);
        //binding compare click
        $("#btnCompareInterview").on('click', SearchField.compareClick);

    },
    //loading interviewers from server
    loadData: function (request, response) {
        $.ajax({
            url: SearchField._properties.interviewersUrl,
            data: "{ 'key': '" + request.term + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                response(data);
                if (data.length > 0) {
                    if (data.length > 2) {
                        $("#tableButtons").css("top", "186px");
                    }
                    else if (data.length === 2) {
                        $("#tableButtons").css("top", "145px");
                    }
                    else if (data.length === 1) {
                        $("#tableButtons").css("top", "100px");
                    }
                }

                SearchField._properties.selectedItems.forEach(function (id) {
                    $('.checkbox-interviewer[value=' + id + ']').prop('checked', true);
                });
            }
        });
    },
    //render eachItem
    renderItem: function (ul, item) {
        if (item.Id === "-1") {
            return $('<li>')
                .data('item.autocomplete', item)
                .append('<table  width="100%" style="margin-left: 4px;"><tbody id= "templateNoResult" ><tr><td colspan="2" class="text-center"><span>No result found!</span>' +
                '</td></tr></tbody ></table >')
                .appendTo(ul);
        } else {
            return $('<li>')
                .data('item.autocomplete', item)
                .append('<table width="100%" style="margin-left: 4px;"><tbody id="templateRow"><tr><td class="interviewer-name"><span>'
                + item.InterviewerName + '</span></td><td class="interviewer-check">'
                + '<input type="checkbox" class="checkbox-interviewer" value="' + item.Id + '" /></td></tr></tbody ></table>')
                .appendTo(ul);
        }

    },
    //add no result
    response: function (event, ui) {
        if (!ui.content.length) {
            var noResult = { Id: "-1" };
            ui.content.push(noResult);
            $("#tableButtons").hide();
        }
    },
    //select item event
    select: function (event, ui) {

        var id = ui.item.Id;
        var index = SearchField._properties.selectedItems.indexOf(id);

        var checked = $('.checkbox-interviewer[value=' + id + ']').prop('checked');//check or uncheck

        if (checked && index < 0) {
            if (SearchField._properties.selectedItems.length > 2) {
                event.preventDefault();
                return false;
            }
            SearchField._properties.selectedItems.push(id);
        } else if (!checked && index >= 0) {
            SearchField._properties.selectedItems.splice(index, 1);
        }

        if (SearchField._properties.selectedItems.length > 0) {

            if (SearchField._properties.selectedItems.length === 1) {
                $("#btnSearchInterview").show();
                $("#btnCompareInterview").hide();

            } else {
                $("#btnSearchInterview").hide();
                $("#btnCompareInterview").show();
            }

            $("#tableButtons").show();
        } else {
            $("#tableButtons").hide();
        }
        return false;
    },
    //fixing list position
    fixPosition: function () {
        $('#ui-id-1').position({
            of: $('#ui-id-1'),
            my: 'left-25',
            at: 'left'
        });
    },
    //closing event
    close: function (e) {
        if (SearchField._properties.readyToClose) {
            //clear selection
            SearchField._properties.selectedItems = [];
            //hide buttons
            $("#tableButtons").hide();

            clearTimeout(this.closing), this.menu.element.is(":visible") &&
                (this.menu.element.hide(), this._trigger("close", e));
        }
        else
            return false;
    },
    searchClick: function () {
        if (SearchField._properties.selectedItems.length === 1) {
            window.location = SearchField._properties.interviewerProfileUrl +
                "/" +
                SearchField._properties.selectedItems[0];
        }
    },
    compareClick: function () {
        if (SearchField._properties.selectedItems.length > 1) {
            if (SearchField._properties.standingsPage === "True") {
                debugger;
                CompareLogic.markCompareAndSort(SearchField._properties.selectedItems);
            } else {
                window.location = SearchField._properties.standingsUrl +
                    "?comparsing=" +
                    SearchField._properties.selectedItems.join(";");
            }
        }
    }
}