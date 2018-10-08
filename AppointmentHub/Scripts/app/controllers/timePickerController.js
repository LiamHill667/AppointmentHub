var TimePickerController = function (TimePickerService) {
    var timePicker;

    var init = function (container) {
        $(container).on("change", ".js-start-time-picker", startTimeChanged);
    };

    var startTimeChanged = function (e) {
        timePicker = $(e.target);
        var startTime = timePicker.val();
        TimePickerService.startTimeChanged(startTime, done, fail);

    };

    var done = function (data) {
        var markup = "";
        for (var x = 0; x < data.length; x++) {
            markup += "<option value=" + data[x] + ">" + data[x] + "</option>";
        }
        $("#EndTime").html(markup).show();
    };

    var fail = function () {
        alert("Something failed");
    };

    return {
        init: init
    }
}(TimePickerService);