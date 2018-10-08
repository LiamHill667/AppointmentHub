var TimePickerService = function () {

    var startTimeChanged = function (startTime, done, fail) {
        $.ajax({
            type: "POST",
            url: "/api/ValidEndTime",
            data: "'" + startTime + "'",
            contentType: "application/json"
        })
            .done(function (data) { done(data) })
            .fail(fail);
    };

    return {
        startTimeChanged: startTimeChanged
    };
}();