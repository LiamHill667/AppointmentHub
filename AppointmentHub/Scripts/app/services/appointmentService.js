var AppointmentService = function () {

    var cancelAppointment = function (appointmentId, done, fail) {
        $.ajax({
            type: "POST",
            url: "/api/Appointment/",
            data: appointmentId,
            contentType: "application/json"
        })
            .done(done)
            .fail(fail);
    };

    return {
        cancelAppointment: cancelAppointment
    };
}();