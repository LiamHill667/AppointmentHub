var MineController = function (AppointmentService) {
    var cancelButton;

    var init = function (container) {
        $(container).on("click", ".js-cancel-appointment", cancelAppointment);
    };

    var cancelAppointment = function (e) {
        cancelButton = $(e.target);
        var appointmentId = cancelButton.attr("data-appointment-id");
        AppointmentService.cancelAppointment(appointmentId, done, fail);
       
    };

    var done = function () {
        cancelButton.parent().parent().parent().remove();
    };

    var fail = function () {
        alert("Something failed");
    };

    return {
        init: init
    }
}(AppointmentService);