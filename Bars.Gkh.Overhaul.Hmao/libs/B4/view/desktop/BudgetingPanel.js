Ext.define('B4.view.desktop.BudgetingPanel', {
    extend: 'B4.view.desktop.BaseCrStatisticWidgetPanel',
    alias: 'widget.budgetingpanel',

    widgetTitle: 'Бюджетирование',

    getWidget: function () {
        var me = this;

        return {
            xtype: 'highchart',
            highchartCfg: {
                chart: {
                    type: 'column',
                    backgroundColor: '#ecf2f4'
                },
                title: false,
                xAxis: {
                    type: 'category',
                    title: {
                        text: null
                    },
                    min: 0,
                    max: 20,
                    scrollbar: {
                        enabled: true
                    },
                    tickLength: 0,
                    labels: {
                        formatter: function(ctx){
                            return ctx.value && Ext.isString(ctx.value) && ctx.value.length === 4 ? ctx.value : '';
                        },
                        rotation: 90,
                        style: {
                            fontSize: '0.7em',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Финансирование, рубли'
                    }
                },
                series: [{
                    groupPadding: 0
                }],
                legend: {
                    enabled: false
                },
                tooltip: {
                    headerFormat: '<span style="font-size:12px"><b>{point.key}</span></b><br/>',
                    pointFormat: '{point.y} руб.'
                },
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