var AvailabilityService = function () {

    var removeAvailability = function (availabilityId, done, fail) {
        $.ajax({
            url: "/api/availability/" + availabilityId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        removeAvailability: removeAvailability
    };
}();