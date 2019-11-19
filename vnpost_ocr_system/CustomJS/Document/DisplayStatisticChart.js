function SubmitForm(form) {
    $("#pre-load").show("slow", function () { });
    $("#pre-load").css("z-index", "99999");
    $("#pre-load").show();

    $.validator.unobtrusive.parse(form);

    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: form.action,
            data: $(form).serialize(),
            success: function (response) {
                var d = response.date;
                $('#talbe').find('tr:eq(2)').find('td:eq(0)').text(d);
                $('#talbe').find('tr:eq(2)').find('td:eq(1)').text(response.odb.total_cho);
                $('#talbe').find('tr:eq(2)').find('td:eq(2)').text(response.odb.total_da);
                $('#talbe').find('tr:eq(2)').find('td:eq(3)').text(response.odb.total_xong);
                getValue();
                update();
                $("#pre-load").hide("slow", function () {
                });

            }
        });
    }

    return false;
}
function SubmitForm2(form) {
    $("#pre-load").show("slow", function () { });
    $("#pre-load").css("z-index", "99999");
    $("#pre-load").show();

    $.validator.unobtrusive.parse(form);

    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: form.action,
            data: $(form).serialize(),
            success: function (response) {
                var d = response.date;
                $('#talbe').find('tr:eq(3)').find('td:eq(0)').text(d);
                $('#talbe').find('tr:eq(3)').find('td:eq(1)').text(response.odb.total_cho);
                $('#talbe').find('tr:eq(3)').find('td:eq(2)').text(response.odb.total_da);
                $('#talbe').find('tr:eq(3)').find('td:eq(3)').text(response.odb.total_xong);
                getValue();
                update();
                $("#pre-load").hide("slow", function () {
                });

            }
        });
    }

    return false;
}
function SubmitFormAll(form) {
    $("#pre-load").show("slow", function () { });
    $("#pre-load").css("z-index", "99999");
    $("#pre-load").show();

    $.validator.unobtrusive.parse(form);

    if ($(form).valid()) {
        $.ajax({
            type: "POST",
            url: form.action,
            data: $(form).serialize(),
            success: function (response) {
                $("#pre-load").hide("slow", function () {
                });

            }
        });
    }

    return false;
}

function change1() {
    var job_start_date = $('#start1').val();
    var job_end_date = $('#end1').val();
    job_start_date = job_start_date.split('/');
    job_end_date = job_end_date.split('/');

    var new_start_date = new Date(job_start_date[2], job_start_date[1], job_start_date[0]);
    var new_end_date = new Date(job_end_date[2], job_end_date[1], job_end_date[0]);

    if (new_start_date > new_end_date) {
        alert("Khoảng thời gian không tồn tại.");
    } else {
        $('#form1').submit();
    }
}

function change2() {
    var job_start_date = $('#start2').val();
    var job_end_date = $('#end2').val();
    job_start_date = job_start_date.split('/');
    job_end_date = job_end_date.split('/');

    var new_start_date = new Date(job_start_date[2], job_start_date[1], job_start_date[0]);
    var new_end_date = new Date(job_end_date[2], job_end_date[1], job_end_date[0]);

    if (new_start_date > new_end_date) {
        alert("Khoảng thời gian không tồn tại.");
    } else {
        $('#form2').submit();
    }
}

$(".date1").datepicker({
    onSelect: function () {
        //alert("absd");
        var job_start_date = $('#start1').val();
        var job_end_date = $('#end1').val();
        job_start_date = job_start_date.split('/');
        job_end_date = job_end_date.split('/');

        var new_start_date = new Date(job_start_date[2], job_start_date[1], job_start_date[0]);
        var new_end_date = new Date(job_end_date[2], job_end_date[1], job_end_date[0]);

        if (new_start_date > new_end_date) {
            alert("Khoảng thời gian không tồn tại.");
        } else {
            $('#form1').submit();
        }
    }
});
$(".date2").datepicker({
    onSelect: function () {
        //alert("absd");
        var job_start_date = $('#start2').val();
        var job_end_date = $('#end2').val();
        job_start_date = job_start_date.split('/');
        job_end_date = job_end_date.split('/');

        var new_start_date = new Date(job_start_date[2], job_start_date[1], job_start_date[0]);
        var new_end_date = new Date(job_end_date[2], job_end_date[1], job_end_date[0]);

        if (new_start_date > new_end_date) {
            alert("Khoảng thời gian không tồn tại.");
        } else {
            $('#form2').submit();
        }

    }
});
function getValue() {
    d1 = $('#talbe').find('tr:eq(2)').find('td:eq(0)').text();
    d2 = $('#talbe').find('tr:eq(3)').find('td:eq(0)').text();

    cho1 = $('#talbe').find('tr:eq(2)').find('td:eq(1)').text();
    cho2 = $('#talbe').find('tr:eq(3)').find('td:eq(1)').text();

    da1 = $('#talbe').find('tr:eq(2)').find('td:eq(2)').text();
    da2 = $('#talbe').find('tr:eq(3)').find('td:eq(2)').text();

    xong1 = $('#talbe').find('tr:eq(2)').find('td:eq(3)').text();
    xong2 = $('#talbe').find('tr:eq(3)').find('td:eq(3)').text();
}
function update() {

    "use strict";
    // ------------------------------
    // Basic bar chart
    // ------------------------------
    // based on prepared DOM, initialize echarts instance
    var myChart = echarts.init(document.getElementById('basic-bar'));

    // specify chart configuration item and data
    var option = {
        // Setup grid
        grid: {
            left: '1%',
            right: '15%',
            bottom: '3%',
            containLabel: true
        },

        // Add Tooltip
        tooltip: {
            trigger: 'axis'
        },



        color: ['#d675d0', '#58bbff', '#fcb051'],
        calculable: true,
        xAxis: [
            {
                name: 'Khoảng thời gian',
                type: 'category',
                data: [d1, d2]
            }
        ],
        yAxis: [
            {
                name: 'Số lượng',
                type: 'value',
                data: []
            }
        ],
        series: [
            {
                name: 'Hồ sơ đang chờ tiếp nhận',
                type: 'bar',
                data: [cho1, cho2],


            },
            {
                name: 'Hồ sơ đã tiếp nhập',
                type: 'bar',
                data: [da1, da2],


            },
            {
                name: 'Hồ sơ đã xử lý',
                type: 'bar',
                data: [xong1, xong2],


            }

        ]
    };
    // use configuration item and data specified to show chart
    myChart.setOption(option);





    //------------------------------------------------------
    // Resize chart on menu width change and window resize
    //------------------------------------------------------
    $(function () {

        // Resize chart on menu width change and window resize
        $(window).on('resize', resize);
        $(".sidebartoggler").on('click', resize);

        // Resize function
        function resize() {
            setTimeout(function () {

                // Resize chart
                myChart.resize();
                stackedChart.resize();
                stackedbarcolumnChart.resize();
                barbasicChart.resize();
            }, 200);
        }
    });
}

