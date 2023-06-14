(function () {
    $(window).on("load", function () {

        setTimeout(function(){
            $(".loader").hide();
        }, 3500);

        $(".modal-body").mCustomScrollbar({
            theme: "minimal-dark"
        });
        
        $(".description > .modal-body").mCustomScrollbar({
            theme: "minimal-dark" //,
        });

        $(".chem-elem-desc .modal-body").mCustomScrollbar({
            theme: "minimal-dark"
        });

        $(document).keyup(function (e) {
            if (e.keyCode == 27) { 
                $(".description").hide();
            }
        });

        $(".indicators-list > ul li span").on("click", function () {
            var div = $(this).parent("li").children(".content");
            if ($(div).css("display") == "block") {
                $(div).hide();
            } else {
                $(div).show();
            }
        });
        $(".indicators-list li ul i").on("click", function () {
            var div = $(this).parent("li").children(".content");
            if ($(div).css("display") == "block") {
                $(div).hide();
            } else {
                $(div).show();
            }
        });

        $(".add-btn").on("click", function () {
            $("#fields").hide();
            $("#input").show();
        });

        $(window).on('resize', function () {
            var win = $(this); //this = window
            if (win.width() >= 1025) {
                $(".side-main-menu").hide();
            }
        });

        $(".navmain a").on("click", function () {
            $(".indicators-list > ul > div").hide();
            var classesNameOfCategoryItem = $(this).attr("class");
            if (classesNameOfCategoryItem != undefined) {
                var idOfBlockForShow = "#" + classesNameOfCategoryItem.replace("active", "").replace(".", "");
            }

            $(idOfBlockForShow).show();
            $(this).parent("li").parent("ul").find("a").removeClass("active");
            $(this).addClass("active");

        });

        $(".main-agro-indicators ul li input").on("click", function () {
            $(".legend-tooltip").show();
            //var tableLegendClassName = $(".main-agro-indicators input:checked").attr("class").split("-")[0] + "-legend";
            //$(".legend-tooltip table").empty();
            //var table = document.createElement("table");
            //$(table).append($("." + tableLegendClassName).html());
            //$(".legend-tooltip table").append(table);
        });

        $("#open-menu-btn").on("click", function () {
            $(".side-main-menu").show();
        });

        $("#close-menu-btn").on("click", function () {
            $(".side-main-menu").hide();
        });

        $("#close-tooltip-btn").on("click", function () {
            $(".legend-tooltip").hide();
            $(".description").hide();
        });

        $("#close-desc-btn").on("click", function () {
            $(".description").hide();
        });

        $("#subscribe-btn").on("click", function () {
            if (!validateEmail($("#susb-email").val())) {
                $(".container-fluid").append("<div class='alert alert-danger alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Помилка. Ви ввели email неправильного формату </strong></div>");
                $(".alert.alert-danger").delay(5000).fadeOut(400);
                return false;
            }

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/Welcome/Subscribe",
                data: { Email: $("#susb-email").val() },
                success: function (data) {
                    $(".container-fluid").append("<div class='alert alert-success alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong> " + data.Email + " </strong>Ви успішно підписані на на відкриття нашого сайту</div>");
                    $(".alert.alert-success").delay(5000).fadeOut(400);
                },
                error: function (data) {
                    $(".container-fluid").append("<div class='alert alert-danger alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Помилка</strong>Ви не були підписані. Спробуйте ще раз</div>");
                    $(".alert.alert-danger").delay(5000).fadeOut(400);
                }
            });
            $("#susb-email").val("");
        });

        $("#contact-btn").on("click", function () {
            if (!validateEmail($("#MsgEmail").val())) {
                $(".container-fluid").append("<div class='alert alert-danger alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Помилка. Ви ввели email неправильного формату</strong></div>");
                $(".alert.alert-danger").delay(5000).fadeOut(400);
                return false;
            }

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/Welcome/Contact",
                data: { Email: $("#MsgEmail").val(), Name: $("#MsgName").val(), Text: $("#MsgText").val()},
                success: function (data) {
                    $(".container-fluid").append("<div class='alert alert-success alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong> " + data.Email + " </strong>Ви успішно підписані на на відкриття нашого сайту</div>");
                    $(".alert.alert-success").delay(5000).fadeOut(400);
                },
                error: function (data) {
                    $(".container-fluid").append("<div class='alert alert-danger alert-dismissible'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Помилка</strong>Ви не були підписані. Спробуйте ще раз</div>");
                    $(".alert.alert-danger").delay(5000).fadeOut(400);
                }
            });

            $("#MsgEmail").val("");
            $("#MsgName").val("");
            $("#MsgText").val("");
        });

        function validateEmail(email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }
    });
})();
