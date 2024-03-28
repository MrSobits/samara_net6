Ext.define('B4.view.regressionanalysis.ChartUnit', {
    extend: 'Ext.chart.Chart',
    requires: [
        'Ext.chart.*',
        'Ext.fx.target.Sprite'
    ],

    alias: 'widget.regressionanalysischartunit',

    name: 'RegressionAnalazysChart',
    style: 'background:#fff',
    border: false,
    animate: true,
    shadow: true,
    theme: 'Category1',
    legend: {
        position: 'right'        
    },
    
    axes: [
    {
        type: 'Numeric',
        position: 'left',
        minorTickSteps: 1,                    
        title: 'Значение индикатора',
        label: {
            rotate: {
                degrees: 315
            }
        },
        minimum: 0,
        grid: {
            odd: {
                opacity: 1,
                fill: '#ddd',
                stroke: '#bbb',
                'stroke-width': 0.5
            }
        }
    },
    {
        type: 'Category',
        position: 'bottom',
        fields: ['month'],                    
        title: 'Месяцы из выбранного периода'
    }
    ],
    series: []
});