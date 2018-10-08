$(document).ready(function () {
    $.get("/Notification/NotificationList", function (notifications) {

        $.get("/Notification/NotificationList", function (notifications) {

            $(".notifications").replaceWith(notifications);

            $(".notifications").on("click", function () {
                $.post("/api/notifications/markAsRead")
                    .done(function () {
                        $(".js-notifications-count")
                            .text("0");
                        //$(".notifications .dropdown-menu")
                        //    .remove()
                    });
            });

        });
    });

    $(".date-control").datepicker({
        dateFormat: "dd/mm/yy"
    });

});