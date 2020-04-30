$(document).ready(function () {
    $('.tk1').hide(); $('.tk2').hide();$('.tk3').hide(); $('.tk4').hide();
    $('#item1').change(function () {
        $('#pre-load').show()
        //var text = $("#item1 option:selected").html();
        //city(text);
        //$('.tk1').show();
        $.ajax({
            type: "POST",
            url: "/GetDistrict",
            cache: false,
            dataType: 'json',
            data: {
                provinceID: $(this).val()
            },
            success: function (response) {
                $('#item2').empty();
                var option = "<option value='0' selected>Tất cả</option>";
                for (var i = 0; i < response.length; i++) {
                    option += "<option value='" + response[i].PostalDistrictCode + "'>" + response[i].PostalDistrictName + "</option>";
                }
                $('#item2').append(option);
                $('#item2').formSelect();
                $('#pre-load').hide(); 
                
            },
            error: function () {
                $('#pre-load').hide()
            }
        })
    })

    $('#item2').change(function () {
        $('#pre-load').show()
        //var text = $("#item2 option:selected").html();
        //district(text);
        //$('.tk2').show();
        $.ajax({
            type: "POST",
            url: "/GetAdmins",
            cache: false,
            dataType: 'json',
            data: {
                code: $(this).val()
            },
            success: function (response) {
                $('#item3').empty();
                var option = "<option value='0' selected>Tất cả</option>";
                for (var i = 0; i < response.length; i++) {
                    option += "<option value='" + response[i].PublicAdministrationLocationID + "'>" + response[i].PublicAdministrationName + "</option>";
                }
                $('#item3').append(option);
                $('#item3').formSelect();

                $('#pre-load').hide()
            },
            error: function () {
                $('#pre-load').hide()
            }
        })
    })

    $('#item3').change(function () {
        $('#pre-load').show()
        //var text = $("#item3 option:selected").html();
        //org(text);
        //$('.tk3').show();
        $.ajax({
            type: "POST",
            url: "/GetProfile",
            cache: false,
            dataType: 'json',
            data: {
                code: $(this).val()
            },
            success: function (response) {
                $('#item4').empty();
                var option = "<option value='0' selected>Tất cả</option>";
                for (var i = 0; i < response.length; i++) {
                    option += "<option value='" + response[i].ProfileID + "'>" + response[i].ProfileName + "</option>";
                }
                $('#item4').append(option);
                $('#item4').formSelect();
                $('#pre-load').hide()
            },
            error: function () {
                $('#pre-load').hide()
            }
        })
    })

    //$('#item4').change(function () {
    //    var text = $("#item4 option:selected").html();
    //    procedure(text);
    //    $('.tk4').show();
    //})
});

//function city(value) {

//    document.getElementById('city').innerHTML = value;
//}
//function district(value) {

//    document.getElementById('district').innerHTML = value;
//}
//function org(value) {

//    document.getElementById('org').innerHTML = value;
//}
//function procedure(value) {

//    document.getElementById('procedure').innerHTML = value;
//}


