Ext.define('B4.view.desktop.CrWorksInCeoContextPanel', {
    extend: 'B4.view.desktop.BaseCrStatisticWidgetPanel',
    alias: 'widget.crworksinceocontextpanel',

    requires: [
        'Ext.ux.Highchart',
    ],

    widgetTitle: 'Количество работ ДПКР в разрезе ООИ',
    
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
                    'Количество работ: <b>{point.y}</b><br/>' +
                    '<div style="font-size:1.5em; color: {point.color}; font-weight: bold; text-align: center">{point.percentage:.1f}%</div>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    size: '100%',
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            legend: {
                enabled: true,
                alignColumns: true,
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                itemMarginBottom: 4,
                width: '40%'
            },
            series: [{
                innerSize: '20%',
                borderRadius: 5
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