$(function () {
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
                data: ['03/10/2018 - 10/10/2018','05/11/2018 - 12/11/2018']
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
                data: [30,45],
                
                
            },
            {
                name: 'Hồ sơ đã tiếp nhập',
                type: 'bar',
                data: [12,56],
               
               
            },
            {
                name: 'Hồ sơ đã xử lý',
                type: 'bar',
                data: [24, 33],


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
});