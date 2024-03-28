Ext.define('B4.utils.Highcharts', {
    singleton: true,
    requiers: [
        'B4.enums.efficiencyrating.DiagramType'
    ],

    getConfiguredHighchart: function(container, data) {
        var categories = Ext.Array.map(data.series[0].data, function(el) { return el[0] }),
            step = 100 / data.series.length,
            position = step / 2,
            dataChart = Ext.clone(data);

        Ext.Array.each(dataChart.series,
            function(el) {
                el.center = [Ext.String.format('{0}%', position)];

                el.title = {
                    text: Ext.String.format('<b>{0}</b>', el.name),
                    verticalAlign: 'bottom',
                    align: 'center',
                    y: 40
                };

                position = position + step;
            });

        var chart = new Highcharts.Chart({
            chart: {
                type: dataChart.type,
                renderTo: container.getItemId()
            },

            xAxis: {
                minPadding: 0.08,
                maxPadding: 0.08,
                categories: categories,
                title: {
                    text: dataChart.xFieldTitle
                }
            },
            yAxis: {
                title: {
                    text: dataChart.yFieldTitle
                }
            },

            title: {
                text: dataChart.title
            },
            series: dataChart.series,
            credits: {
                enabled: false
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y:.2f}</b>'
            },
            plotOptions: {
                pie: {
                    size: '60%',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.y:.2f}'
                    }
                }
            },

            exporting: {
                plotOptions: {
                    pie: {
                        size: '60%',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.y:.2f}'
                        }
                    }
                },
                enabled: true,
                fallbackToExportServer: false,
                buttons: {
                    contextButton: {
                        enabled: false
                    },
                    exportButton: {
                        text: 'Скачать',
                        menuItems: [
                            {
                                text: 'Скачать в формате PNG',
                                onclick: function() {
                                    this.exportChart();
                                }
                            },
                            {
                                text: 'Скачать в формате JPEG',
                                onclick: function() {
                                    this.exportChart({
                                        type: 'image/jpeg'
                                    });
                                }
                            },
                            {
                                text: 'Скачать в формате SVG',
                                onclick: function() {
                                    this.exportChart({
                                        type: 'image/svg+xml'
                                    });
                                }
                            },
                            {
                                text: 'Скачать в формате PDF',
                                onclick: function() {
                                    this.exportChart({
                                        type: 'application/pdf'
                                    });
                                }
                            }
                        ]
                    },

                    printMaxWidth: 780
                }
            }
        });

        return container.chart = chart;
    },

    getEnumType: function(type) {
        switch(type) {
            case 'spline':
                return B4.enums.efficiencyrating.DiagramType.LineDiagram;

            case 'column':
                return B4.enums.efficiencyrating.DiagramType.BarDiagram;

            case 'areaspline':
                return B4.enums.efficiencyrating.DiagramType.LogarithmicChart;

            case 'pie':
                return B4.enums.efficiencyrating.DiagramType.PieGraph;
        }

        return null;
    }
});