$(document).ready(function () {

    //>>>>>>>>>>>>>>>>> Global variable initialization on start <<<<<<<<<<<<<<<<<<<<<<<<<
    var host = window.location.host;
    var token = null;
    var headers = {};
    var picturesEndpoint = "/api/pictures";
    var galeriesEndpoint = "/api/galleries";

    var editingId;

    $("#logoutDiv").css("display", "none");
    $("#regForm").css("display", "none");
    $("#loginDiv").css("display", "none");

    var picturesUrl = "http://" + host + picturesEndpoint;
    var galeriesUrl = "http://" + host + galeriesEndpoint;
    var pretragaUrl = "http://" + host + "/api/search";

    $.getJSON(picturesUrl, loadMainEntity);

    $("body").on("click", "#logoutBtn", reset);
    $("body").on("click", "#giveUpBtn", cleanForm);
    $("body").on("click", "#prijavaBtn", loadLoginForm);
    $("body").on("click", "#btnDelete", deletepicture);


    $("body").on("click", "#loginBtn", ulogujSe);

    $("body").on("click", "#findBtn", pretrazi);
    $("body").on("click", "#regFormLoad", loadRegistration);


    $("body").on("click", "#regBtn", reset);

    //>>>>>>>>>>>>>>>> Clean creation form <<<<<<<<<<<<<<

    function cleanForm() {
        $("#createInput1").val('');
        $("#createInput2").val('');
        $("#createInput3").val('');
        $("#createInput5").val('');

    }

    //>>>>>>>> Load registration form <<<<<<<<<<
    function loadRegistration() {
        $("#info").empty();
        $("#btns").addClass("hidden");
        $("#loginDiv").css("display", "none");
        $("#regForm").css("display", "block");
    }
    //>>>>>>>> Load login form <<<<<<<<<<
    function loadLoginForm() {
        $("#info").empty();
        $("#loginDiv").css("display", "block");
        $("#btns").addClass("hidden");
        $("#regForm").css("display", "none");
    }

    //>>>>>>>> Load login form <<<<<<<<<<
    $("body").on("click", "#jumpToLogin", reset);

    //>>>>>>>>>>>>>>> Reset/logout <<<<<<<<<<<<<<<
    function reset() {
        if (token != null) {
            token = null;
        }
        $("#loginDiv").css("display", "none");
        $("#regForm").css("display", "none");
        $("#logoutDiv").css("display", "none");
        $("#loggedInParagraph").empty();
        $("#btns").removeClass("hidden");
        $("#brDiv").addClass("hidden");
        $("#pFirst").empty();
        $("#pSecond").empty();
        $("#create").addClass("hidden");
        $("#brDiv").addClass("hidden");
        $("#search").addClass("hidden");
        $("#findInput1").val('');
        $("#findInput2").val('');




        $.getJSON(picturesUrl, loadMainEntity);
    }

    //>>>>>>>>>>> Registration <<<<<<<<<<<

    $("#registration").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();
        var loz1 = $("#regPass").val();
        var loz2 = $("#regPass2").val();


        var sendData = {
            "Email": email,
            "Password": loz1,
            "ConfirmPassword": loz2
        };

        $.ajax({
            type: "POST",
            url: 'http://' + host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            $("#info").append("Successful registration. You can log in to the system.");
            $("#regEmail").val('');
            $("#regPass").val('');
            $("#regPass2").val('');
            $("#regForm").css("display", "none");
            $("#loginDiv").css("display", "block");

        }).fail(function (data) {
            alert("Greska prilikom registracije!");
        });
    });

    //>>>>>>>>>>>>>> Adding main entity(picture) <<<<<<<<<<<<<<<<<<<<<<<<<

    $("#create").submit(function (e) {

        e.preventDefault();



        var name = $("#createInput1").val();
        var author = $("#createInput2").val();
        var year = $("#createInput3").val();
        var gallery = $("#createInput4select").val();
        var price = $("#createInput5").val();

        $("#validationMsgInput1").empty();
        $("#validationMsgInput2").empty();
        $("#validationMsgInput3").empty();
        $("#validationMsgInput5").empty();




        if (token) {
            headers.Authorization = "Bearer " + token;
        }

        var dataCreate = {
            "Name": name,
            "Author": author,
            "MadeYear": year,
            "GalleryId": gallery,
            "Price": price,

        }
        httpAction = "POST";

        $.ajax({
            "url": picturesUrl,
            "type": httpAction,
            "data": dataCreate,
            "headers": headers
        })
            .done(function (data, status) {
                $.getJSON(picturesUrl, loadMainEntity);
                $("#createInput1").val('');
                $("#createInput2").val('');
                $("#createInput3").val('');
                $("#createInput5").val('');



            })
            .fail(function (data, status) {
                validation();
                //alert("Greska prilikom dodavanja!");
            })

    })

    //>>>>>>>>>>> Login <<<<<<<<<<<<<<<<<<

    function ulogujSe() {

        var email = $("#loginEmail").val();
        var loz = $("#loginPass").val();

        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            "type": "POST",
            "url": 'http://' + host + "/Token",
            "data": sendData

        }).done(function (data) {

            $("#info").empty().html("Logged in user: <b>" + data.userName + "</b>");

            token = data.access_token;
            console.log(token);
            $("#loginEmail").val('');
            $("#loginPass").val('');
            $("#loginDiv").css("display", "none");
            $("#regForm").css("display", "none");
            $("#loggedInParagraph").html("Logged in user: <b>" + email + "</b>");
            $("#logoutDiv").css("display", "block");
            $("#brDiv").removeClass("hidden");



            $.getJSON(picturesUrl, loadMainEntity);
            $.getJSON(galeriesUrl, getgaleries);

            $("#data").css("display", "block");
            $("#create").removeClass("hidden");
            $("#search").removeClass("hidden");

        }).fail(function (data) {
            alert("Greska prilikom prijave!");
        });

    };

    //>>>>>>>>>>>>>>>> Load 2nd entity into dropdown menu-create form <<<<<<<<<<<<<<<<<<

    function getgaleries(data, status) {
        var galeries = $("#createInput4select");
        galeries.empty();

        if (status === "success") {

            for (var i = 0; i < data.length; i++) {
                var option = "<option value=" + data[i].Id + ">" + data[i].Name + "</option>";
                galeries.append(option);
            }
        }
        else {
            var div = $("<div></div>");
            var h3 = $("<h3>An error occurred while downloading the gallery!</h3>");
            div.append(h3);
            galeries.append(div);
        }



    }

    //>>>>>>>>>>>>>>>>>> Load table with main entity <<<<<<<<<<<<<<<<<<<<<<
    function loadMainEntity(data, status) {
        console.log("Status: " + status);
        $("#data").empty();



        var container = $("#data");
        container.empty();

        if (status == "success") {
            console.log(data);

            // ispis naslova
            var div = $("<div></div>");
            var h1 = $("<h1>Pictures</h1>");
            var head = $("<thead></thead>");
            var body = $("<tbody></tbody>");

            div.append(h1);

            var table = $("<table border='1'  class=\"table table-hover text-center\" ></table>");
            var header = $("<tr style=\"background-color : lightblue; height:20px\"><th class=\"text-center\" style=\"width:200px\">Name</th><th class=\"text-center\" style=\"width:250px\">Author</th><th class=\"text-center\" style=\"width:100px\">Price</th><th class=\"text-center\" style=\"width:200px\">Gallery</th><th class=\"text-center hidden\" style=\"width:120px\">Action</th></tr>");
            head.append(header);
            table.append(head);
            table.append(body);


            for (i = 0; i < data.length; i++) {

                // prikazujemo novi red u tabeli
                var row = "<tr style=\"height:20px\">";
                // prikaz podataka
                var displayData = "<td>" + data[i].Name + "</td><td>" + data[i].Author + "</td><td>" + data[i].Price + "</td><td>" + data[i].GalleryName + "</td>";
                // prikaz dugmadi za izmenu i brisanje
                var stringId = data[i].Id.toString();
                console.log(stringId);
                var displayDelete = "<td class=\"hidden\"><a href=\"#\" id=btnDelete name=" + stringId + ">[delete]</a></td>";
            


                row += displayData + displayDelete + "</tr>";
                body.append(row);
                //table.append(row);

            }

            div.append(table);

            // ispis novog sadrzaja
            container.append(div);
            if (token) {
                $("th").removeClass("hidden");
                $("td").removeClass("hidden");
                $("#brDiv").removeClass("hidden");
                $("#findInput1").val('');
                $("#findInput2").val('');


            }
        }
        else {
            var div = $("<div></div>");
            var h1 = $("<h1>An error occurred while adding new picture</h1>");
            div.append(h1);
            container.append(div);
        }
    }

    //>>>>>>>>>>>>>>>>>>>> Removing entry from table od button delete <<<<<<<<<<<<<<<<<<<<<<<
    function deletepicture() {
        var deleteId = this.name;
        console.log(this.name);
        httpAction = "DELETE";

        if (token) {
            headers.Authorization = "Bearer " + token;
        }
        var picturesUrl = "http://" + host + picturesEndpoint;
        $.ajax({
            "url": picturesUrl + "?id=" + deleteId,
            "type": httpAction,
            "headers": headers

        })
            .done(function (data, status) {
                picturesUrl = "http://" + host + picturesEndpoint;
                $.getJSON(picturesUrl, loadMainEntity);

            })
            .fail(function (data, status) {

                alert("An error occurred while deleting the picture!")
            })

    };

    //>>>>>>>>>>>>>>>>>>>>>> Search form <<<<<<<<<<<<<<<<<<<<<<<<<
    function pretrazi() {
        var start = $("#findInput1").val();
        var kraj = $("#findInput2").val();
        httpAction = "POST";

        if (token) {
            headers.Authorization = "Bearer " + token;
        }

        var searchUrl = pretragaUrl + "?min=" + start + "&max=" + kraj;
        $.ajax({
            "url": searchUrl,
            "type": httpAction,
            "headers": headers
        })
            .done(loadMainEntity)
            .fail(function (data, status) {
                alert("An error occurred while searching the photo!");
            });

    };


});



//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Data validation before creating an object and submiting it to controller <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
function validation() {
    var name = $("#createInput1").val();
    var year = $("#createInput3").val();
    var author = $("#createInput2").val();
    var price = $("#createInput5").val();

    var pName = $("#validationMsgInput1");
    var pAuthor = $("#validationMsgInput2");
    var pYear = $("#validationMsgInput3");
    var pPrice = $("#validationMsgInput5");



    var isValid = true;

    //>>>>>>>>>>>>>> Galery name validation <<<<<<<<<<<<<<<<<<<<<<<<
    if (!name) {
        pName.text("Picture name is a required field!");
        isValid = false;
    }
    else if (name.length > 120) {
        pName.text("Picture name cannot be longer than 120 characters!");
        isValid = false;
    }

    //>>>>>>>>>>>>>> Author name validation <<<<<<<<<<<<<<<<<<<<<<<<
    if (!author) {
        pAuthor.text("The author of the picture is a required field!");
        isValid = false;
    }
    else if (author.length > 70) {
        pAuthor.text("Author name cannot be longer than 70 characters!");
        isValid = false;
    }

    //>>>>>>>>>>>>>> Year validation <<<<<<<<<<<<<<<<<<<<<<<<
    if (!year || year < 1520 || year > 2019) {
        pYear.text("The year must be from the interval from 1520. to 2019.");
        isValid = false;
    }

    //>>>>>>>>>>>>>> Price validation <<<<<<<<<<<<<<<<<<<<<<<<

    if (!price) {
        pPrice.text("Price is a required field!");
        isValid = false;
    }

    else if (price < 100.00 || price > 49999.99) {
        pPrice.text("The price must be from 100 to 49999.99!");
        isValid = false;
    }

    return isValid;
}