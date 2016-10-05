$(document).ready(function () {
    $("button.valueAjaxLink").on('click', function () {
        if ($(this).attr("loaded") === "true") {
            $("#ajaxMenuSearchWait").hide();
        } else {
            var eleId = $(this).attr("data-id");
            var self = $(this);
            $.ajax({
                url: "/File/GetValueComponent",
                data: { elementId: eleId }
            }).done(function (html) {
                self.after(html);
            });
            self.attr("loaded", "true");
            self.hide();
        }
    });
});