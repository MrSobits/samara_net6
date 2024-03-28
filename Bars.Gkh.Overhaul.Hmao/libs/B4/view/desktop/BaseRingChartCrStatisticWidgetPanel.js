Ext.define('B4.view.desktop.BaseRingChartCrStatisticWidgetPanel', {
    extend: 'B4.view.desktop.BaseCrStatisticWidgetPanel',

    requires: [
        'Ext.ux.Highchart'
    ],
    
    // Цвет основного круга
    paneBackgroundColor: '#c0c0c0',
    
    // Прозрачность основного круга
    paneOpacity: 1,
    
    // Цвет доли круга
    seriesBackgroundColor: '#544fc5',

    getWidget: function () {
        var me = this;

        return {
            xtype: 'highchart',
            highchartCfg: {
                chart: {
                    type: 'solidgauge',
                    backgroundColor: '#ecf2f4'
                },
                title: false,
                tooltip: {
                    borderWidth: 0,
                    backgroundColor: 'none',
                    useHTML: true,
                    shadow: false,
                    style: {
                        fontSize: '16px'
                    },
                    pointFormat: '<div style="font-size:2em; color: {point.color}; font-weight: bold; text-align: center">{point.y}%</div>' +
                        '<div style="font-size:1em; color: black; font-weight: bold; text-align: center">{point.x}</div>',
                    positioner: function (labelWidth) {
                        return me.calculateSolidGuageCenter(this, labelWidth);
                    }
                },
                pane: {
                    startAngle: 0,
                    endAngle: 360,
                    background: [{
                        outerRadius: '112%',
                        innerRadius: '68%',
                        backgroundColor: Highcharts.color(this.paneBackgroundColor)
                            .setOpacity(this.paneOpacity)
                            .get(),
                        borderWidth: 0
                    }]
                },
                yAxis: {
                    min: 0,
                    max: 100,
                    lineWidth: 0,
                    tickPositions: []
                },
                plotOptions: {
                    solidgauge: {
                        dataLabels: {
                            enabled: false,
                        },
                        rounded: false
                    }
                },
                series: [{
                    name: 'Move',
                    data: [{
                        color: this.seriesBackgroundColor,
                        radius: '112%',
                        innerRadius: '68%',
                        y: 0
                    }],
                    dataLabels: {
                        format: '{y} %',
                        borderWidth: 0,
                        color: (
                            Highcharts.defaultOptions.title &&
                            Highcharts.defaultOptions.title.style &&
                            Highcharts.defaultOptions.title.style.color
                        ) || '#333333',
                        style: {
                            fontSize: '24px'
                        }
                    },
                }],
                exporting: {
                    enabled: false
                },
                credits: {
                    enabled: false
                }
            },
            flex: 1.5
        };
    }
});