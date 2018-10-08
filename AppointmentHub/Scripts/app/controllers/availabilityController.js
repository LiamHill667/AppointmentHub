var AvailabilityController = function (AvailabilityService) {
    var removeButton;

    var init = function (container) {
        $(container).on("click", ".js-remove-availability", removeAvailability);
    };

    var removeAvailability = function (e) {
        removeButton = $(e.target);
        var availabilityId = removeButton.attr("data-availability-id");
        AvailabilityService.removeAvailability(availabilityId, done, fail);

    };

    var done = function () {
        removeButton.parent().parent().parent().remove();
    };

    var fail = function () {
        alert("Something failed");
    };

    return {
        init: init
    }
}(AvailabilityService);