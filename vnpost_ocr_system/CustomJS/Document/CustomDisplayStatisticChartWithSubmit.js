$(document).ready(function () {
    $('.tk1').hide(); $('.tk2').hide(); $('.tk3').hide(); $('.tk4').hide();

    function getText() {
        var text = $("#item1 option:selected").html();
        $('#provine').val(text);
        $('#provine_ch1').val(text);
        $('#provine_ch2').val(text);
        $('#provine_chy').val(text);
        $('#provine_chm').val(text);

        text = $("#item2 option:selected").html();
        $('#district_c').val(text);
        $('#district_c_ch1').val(text);
        $('#district_c_ch2').val(text);
        $('#district_c_chy').val(text);
        $('#district_c_chm').val(text);

        text = $("#item3 option:selected").html();
        $('#hcc').val(text);
        $('#hcc_ch1').val(text);
        $('#hcc_ch2').val(text);
        $('#hcc_chy').val(text);
        $('#hcc_chm').val(text);

        text = $("#item4 option:selected").html();
        $('#profile').val(text);
        $('#profile_ch1').val(text);
        $('#profile_ch2').val(text);
        $('#profile_chy').val(text);
        $('#profile_chm').val(text);
    }
    function change() {
        $('#pre-load').show()
        var text = $("#item1 option:selected").html();
        if (text == "Tất cả") {
            $('.tk1').hide();
            $('.tk2').hide();
            $('.tk3').hide();
            $('.tk4').hide();
        } else {
            city(text);
            $('.tk1').show();
        }
        $.ajax({
            type: "POST",
            url: "/GetDistrict",
            cache: false,
            dataType: 'json',
            data: {
                code: $(this).val()
            },
            success: function (response) {
                $('#item2').empty();
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                for (var i = 0; i < response.length; i++) {
                    option += "<option value='" + response[i].PostalDistrictCode + "'>" + response[i].PostalDistrictName + "</option>";
                }
                $('#item2').append(option);
                $('#item2').formSelect();

                $('#item3').empty();
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                $('#item3').append(option);
                $('#item3').formSelect();

                $('#item4').empty();
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                $('#item4').append(option);
                $('#item4').formSelect();

                $('.tk2').hide();
                $('.tk3').hide();
                $('.tk4').hide();
                $('#pre-load').hide();

            },
            error: function () {
                $('#pre-load').hide()
            }
        });
        $("div.it2 select").val("Tất cả");
        $("div.it3 select").val("Tất cả");
        $("div.it4 select").val("Tất cả");
        getText();
        $('#formall').submit();
        change1();
        change2();
        $('#form3').submit();
        $("#cmonth").hide();
        $("#cyear").show();
    }
    $('#item1').change(function () {
        $('#pre-load').show()
        var text = $("#item1 option:selected").html();
        if (text == "Tất cả") {
            $('.tk1').hide();
            $('.tk2').hide();
            $('.tk3').hide();
            $('.tk4').hide();
        } else {
            city(text);
            $('.tk1').show();
        }
        $.ajax({
            type: "POST",
            url: "/GetDistrict",
            cache: false,
            dataType: 'json',
            data: {
                code: $(this).val()
            },
            success: function (response) {
                $('#item2').empty();
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                for (var i = 0; i < response.length; i++) {
                    option += "<option value='" + response[i].PostalDistrictCode + "'>" + response[i].PostalDistrictName + "</option>";
                }
                $('#item2').append(option);
                $('#item2').formSelect();

                $('#item3').empty();
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                $('#item3').append(option);
                $('#item3').formSelect();

                $('#item4').empty();
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                $('#item4').append(option);
                $('#item4').formSelect();

                $('.tk2').hide();
                $('.tk3').hide();
                $('.tk4').hide();
                $('#pre-load').hide();

            },
            error: function () {
                $('#pre-load').hide()
            }
        });
        $("div.it2 select").val("Tất cả");
        $("div.it3 select").val("Tất cả");
        $("div.it4 select").val("Tất cả");
        getText();
        $('#formall').submit();
        change1();
        change2();
        $('#form3').submit();
        $("#cmonth").hide();
        $("#cyear").show();
    })

    $('#item2').change(function () {
        $('#pre-load').show()
        var text = $("#item2 option:selected").html();
        if (text == "Tất cả") {
            $('.tk2').hide();
            $('.tk3').hide();
            $('.tk4').hide();
        } else {
            district(text);
            $('.tk2').show();
        }
        
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
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                for (var i = 0; i < response.length; i++) {
                    option += "<option value='" + response[i].PublicAdministrationLocationID + "'>" + response[i].PublicAdministrationName + "</option>";
                }
                $('#item3').append(option);
                $('#item3').formSelect();

                $('#item4').empty();
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                $('#item4').append(option);
                $('#item4').formSelect();

                $('.tk3').hide();
                $('.tk4').hide();
                $('#pre-load').hide()
            },
            error: function () {
                $('#pre-load').hide()
            }
        });
        $("div.it3 select").val("Tất cả");
        $("div.it4 select").val("Tất cả");
        getText();
        $('#formall').submit();
        change1();
        change2();
        $('#form3').submit();
        $("#cmonth").hide();
        $("#cyear").show();
    })

    $('#item3').change(function () {
        $('#pre-load').show()
        var text = $("#item3 option:selected").html();
        if (text == "Tất cả") {
            $('.tk3').hide();
            $('.tk4').hide();
        } else {
            org(text);
            $('.tk3').show();
        }
       
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
                var option = "<option value='Tất cả' selected>Tất cả</option>";
                for (var i = 0; i < response.length; i++) {
                    option += "<option value='" + response[i].ProfileID + "'>" + response[i].ProfileName + "</option>";
                }
                $('#item4').append(option);
                $('#item4').formSelect();

                $('.tk4').hide();
                $('#pre-load').hide()
            },
            error: function () {
                $('#pre-load').hide()
            }
        });
        $("div.it4 select").val("Tất cả");
        getText();
        $('#formall').submit();
        change1();
        change2();
        $('#form3').submit();
        $("#cmonth").hide();
        $("#cyear").show();
    })

    $('#item4').change(function () {
        var text = $("#item4 option:selected").html();
        if (text == "Tất cả") {
            $('.tk4').hide();
        } else {
            procedure(text);
            $('.tk4').show();
        }
        
        getText();
        $('#formall').submit();
        change1();
        change2();
        $('#form3').submit();
        $("#cmonth").hide();
        $("#cyear").show();
    })
});

function city(value) {

    document.getElementById('city').innerHTML = value;
}
function district(value) {

    document.getElementById('district').innerHTML = value;
}
function org(value) {

    document.getElementById('org').innerHTML = value;
}
function procedure(value) {

    document.getElementById('procedure').innerHTML = value;
}


