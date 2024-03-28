Ext.define('B4.view.subsidy.SubsidyMuChart', {
    extend: 'Ext.chart.Chart',

    requires: ['B4.store.subsidy.SubsidyMunicipalityRecord'],
    
    alias: 'widget.subsidymunicipalitychart',

    closable: true,
    
    title: 'Просмотреть график',
    
    store: 'subsidy.SubsidyMunicipalityRecord',
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            shadow: true,
            legend: {
                position: 'top'
            },
            axes: [
                {
                    title: 'Тыс. руб.',
                    type: 'Numeric',
                    position: 'left',
                    fields: ['FinanceNeedFromCorrect', 'BudgetFund'],
                    label: {
                        renderer: function (value) {
                            return Ext.util.Format.currency(value / 1000, null, 2);
                        }
                    },
                    grid: true,
                    minimum: 0
                },
                {
                    title: 'Года',
                    type: 'Numeric',
                    position: 'bottom',
                    fields: ['SubsidyYear']
                }
            ],
            series: [
                {
                    type: 'area',
                    fill: true,
                    highlight: true,
                    axis: 'left',
                    xField: 'SubsidyYear',
                    yField: ['FinanceNeedFromCorrect', 'BudgetFund'],
                    title: ['FinanceNeedFromCorrect', 'BudgetFund'],
                    style: {
                        opacity: 0.35
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});