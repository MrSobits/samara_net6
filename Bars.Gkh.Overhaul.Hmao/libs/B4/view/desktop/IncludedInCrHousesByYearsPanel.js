Ext.define('B4.view.desktop.IncludedInCrHousesByYearsPanel', {
    extend: 'B4.view.desktop.BaseCrStatisticWidgetPanel',
    alias: 'widget.includedincrhousesbyyearspanel',

    requires: [
        'Ext.ux.Highchart',
    ],

    widgetTitle: 'Дома, включенные в ДПКР в разрезе годов',

    getWidget: () => ({
        xtype: 'highchart',
        highchartCfg: {
            chart: {
                type: 'variablepie',
                backgroundColor: '#ecf2f4'
            },
            title: false,
            tooltip: {
                headerFormat: '',
                pointFormat: '<span style="color:{point.color}">\u25CF</span> <b> {point.name}</b><br/>' +
                    'Количество домов: <b>{point.y}</b><br/>' +
                    'Общее количество домов: <b>{point.t}</b><br/>' +
                    '<div style="font-size:2em; color: {point.color}; font-weight: bold; text-align: center">{point.percentage:.1f}%</div>'
            },
            series: [{
                minPointSize: 10,
                innerSize: '20%',
                zMin: 0,
                borderRadius: 5
            }],
            exporting: {
                enabled: false
            },
            credits: {
                enabled: false
            }
        },
        flex: 1.5
    })
});