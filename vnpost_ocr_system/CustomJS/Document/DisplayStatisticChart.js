

$(function () {
    "use strict";
    // ------------------------------
    // Basic line chart
    // ------------------------------
    // based on prepared DOM, initialize echarts instance
    var myChart = echarts.init(document.getElementById('basic-line'));

    // specify chart configuration item and data
    var option = {

        // Setup grid
        grid: {
            left: '1%',
            right: '6%',
            bottom: '4%',
            containLabel: true
        },
        tooltip: {
            trigger: 'axis'
        },


        // Add custom colors
        color: ['#d675d0', '#58bbff', '#fcb051'],

        // Enable drag recalculate
        calculable: true,

        // Horizontal Axiz
        xAxis: [
            {
                name: 'Ngày',
                type: 'category',
                boundaryGap: false,
                data: ['30', '29', '29', '27', '26', '25', '24', '23', '22', '21', '20',
                    '19', '18', '17', '16', '15', '14', '13', '12', '11', '10', '9',
                    '8', '7', '6', '5', '4', '3', '2', '1']
            }
        ],

        // Vertical Axis
        yAxis: [
            {
                name: 'Số lượng',
                type: 'value',
                boundaryGap: false,
                data: ['0', '50', '150', '200', '250', '300', '350', '400', '450', '500', '550', '600']
            }
        ],

        // Add Series
        series: [
            {
                name: 'Đang tiếp nhận',
                type: 'line',
                data: [Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,

                ],


                lineStyle: {
                    normal: {
                        width: 3,
                        shadowColor: 'rgba(0,0,0,0.1)',
                        shadowBlur: 10,
                        shadowOffsetY: 10
                    }
                },
            },
            {

                name: 'Đã tiếp nhận',
                type: 'line',
                data: [Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,

                ],


                lineStyle: {
                    normal: {
                        width: 3,
                        shadowColor: 'rgba(0,0,0,0.1)',
                        shadowBlur: 10,
                        shadowOffsetY: 10
                    }
                },
            },
            {

                name: 'Đã xử lý',
                type: 'line',
                data: [Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,
                       Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500, Math.random() * 500,

                ],


                lineStyle: {
                    normal: {
                        width: 3,
                        shadowColor: 'rgba(0,0,0,0.1)',
                        shadowBlur: 10,
                        shadowOffsetY: 10
                    }
                },
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

            }, 200);
        }
    });
});

//========================================================================================================================
$(function () {
    "use strict";
    // ------------------------------
    // Basic pie chart
    // ------------------------------
    // based on prepared DOM, initialize echarts instance
    var basicpieChart = echarts.init(document.getElementById('basic-pie'));
    var dataStyle = {
        normal: {
            label: { show: false },
            labelLine: { show: false }
        }
    };
    var option = {
        // Add title


        // Add tooltip
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b}: {c} ({d}%)"
        },

        // Add legend
        //legend: {
        //    orient: 'vertical',
        //    x: 'left',
        //    data: ['IE', 'Opera', 'Safari', 'Firefox', 'Chrome']
        //},

        // Add custom colors
        color: ['#d675d0', '#58bbff', '#fcb051'],

        // Display toolbox


        // Enable drag recalculate
        calculable: true,

        // Add series
        series: [{
            name: 'Thống kê',
            type: 'pie',
            radius: '85%',
            center: ['50%', '57.5%'],
            clockWise: false,
            itemStyle: dataStyle,
            data: [
                { value: 240, name: 'Đang tiếp nhận' },
                { value: 320, name: 'Đã tiếp nhận' },
                { value: 200, name: 'Đã xử lý' },

            ]
        }]
    };

    basicpieChart.setOption(option);



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
                basicpieChart.resize();
                poleChart.resize();
            }, 200);
        }
    });
});