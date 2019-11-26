function chartMonth() {
    "use strict";
    // ------------------------------
    // Basic bar chart
    // ------------------------------
    // based on prepared DOM, initialize echarts instance
    var myChart2 = echarts.init(document.getElementById('basic-bar-month'));

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
                name: 'Ngày',
                type: 'category',
                data: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31']
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
                data: chuam,
                
            },
            {
                name: 'Đã tiếp nhận',
                type: 'bar',
                data: dam,
                
            },
            {
                name: 'Đã xử lý',
                type: 'bar',
                data: xongm,
                
            }
        ]
    };
    // use configuration item and data specified to show chart
    myChart2.setOption(option);
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
                myChart2.resize();
                stackedChart.resize();
                stackedbarcolumnChart.resize();
                barbasicChart.resize();
            }, 200);
        }
    });
    //alert("bac");

}