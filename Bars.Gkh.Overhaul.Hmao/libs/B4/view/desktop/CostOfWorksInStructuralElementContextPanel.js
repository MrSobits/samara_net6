Ext.define('B4.view.desktop.CostOfWorksInStructuralElementContextPanel', {
    extend: 'B4.view.desktop.BaseCrStatisticWidgetPanel',
    alias: 'widget.costofworksinstructuralelementcontextpanel',

    requires: [
        'Ext.ux.Highchart'
    ],

    widgetTitle: 'Данные по стоимости работ в разрезе КЭ',

    getWidget: () => ({
        xtype: 'highchart',
        highchartCfg: {
            chart: {
                backgroundColor: 'rgba(255, 255, 255, 0.0)',
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: false,
            tooltip: {
                headerFormat: '',
                pointFormat: '<span style="color:{point.color}">\u25CF</span> <b> {point.name}</b><br/>' +
                    'Сумма работ: <b>{point.y}</b><br/>' +
                    '<div style="font-size:1.5em; color: {point.color}; font-weight: bold; text-align: center">{point.percentage:.1f}%</div>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    size: '120%',
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            if (this.percentage < 4) {
                                return;
                            }
                            
                            return `${(Math.round(this.percentage * 100) / 100).toFixed(1)}%`;
                        },
                        distance: -30,
                        style: {
                            fontWeight: 'bold',
                            color: 'white'
                        }
                    }
                }
            },
            series: [{
                innerSize: '20%',
                borderRadius: 5,
            }],
            exporting: {
                enabled: false
            },
            credits: {
                enabled: false
            }
        },
        flex: 2
    })
});