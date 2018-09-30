function getChartJs(type,data1,data2) {
    var config = null;
    config = {
        type: 'line',
        data: {
            labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dev"],
            datasets: [{
                label: "In",
                data: data1,
                borderColor: '#7264EE',
                pointStyle: 'rectRounded',
                backgroundColor: 'rgba(114, 100, 238, 0.3)',
                pointBorderColor: '#7264EE',
                pointBackgroundColor: 'white',
                borderWidth: 2,
                pointRadius: 4,
                pointHoverRadius: 6,
                pointBorderWidth: 2
            }, {
                label: "Out",
                data: data2,
                borderColor: '#F1212F',
                pointStyle: 'rectRounded',
                backgroundColor: 'rgba(241, 33, 47, 0.3)',
                pointBorderColor: '#F1212F',
                pointBackgroundColor: 'white',
                borderWidth: 2,
                pointRadius: 4,
                pointHoverRadius: 6,
                pointBorderWidth: 2
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {
                            return '$  ' + value;
                        }
                    }
                }]
            },
            elements: {
                line: {
                    tension: 0
                }
            },
            tooltips: {
                enabled: true,
                mode: 'single',
                callbacks: {
                    label: function (tooltipItems, data) {
                        var multistringText = [];
                        if (tooltipItems.datasetIndex == 1) {
                            multistringText.push("Out : $ " + [tooltipItems.yLabel][0]);
                            multistringText.push("In : $ " + data1[tooltipItems.index]);
                        } else if (tooltipItems.datasetIndex == 0) {
                            multistringText.push("In : $ " + [tooltipItems.yLabel][0]);
                            multistringText.push("Out : $ " + data2[tooltipItems.index]);
                        };
                        return multistringText;
                    }
                }
            },
            responsive: true,
            legend: false
        }
    }

    return config;
}