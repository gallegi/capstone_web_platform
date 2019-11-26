function chartYear() {
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

        legend: {
            data: ['Đang tiếp nhận', 'Đã tiếp nhận', 'Đã xử lý']
        },
        color: ['#d675d0', '#58bbff', '#fcb051'],
        calculable: true,
        xAxis: [
            {
                name: 'Tháng',
                type: 'category',
                data: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12']
            }
        ],
        yAxis: [
            {
                name: 'Số lượng',
                type: 'value'
            }
        ],
        series: [
            {
                name: 'Đang tiếp nhận',
                type: 'bar',
                data: chua,
                
            },
            {
                name: 'Đã tiếp nhận',
                type: 'bar',
                data: da,
                
            },
            {
                name: 'Đã xử lý',
                type: 'bar',
                data: xong,
                
            }
        ]
    };
    // use configuration item and data specified to show chart
    myChart.setOption(option);
    myChart.on('click', function (params) {
        $("#hiddenmonth").val(params.name);
        $("#form4").submit();
        $("#cyear").hide();
        $("#cmonth").show();
    });
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
    //alert("bac");

}