Ext.define('B4.model.GasuIndicatorValue', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GasuIndicatorValue'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'GasuIndicator' },
        { name: 'GasuIndicatorName' },
        { name: 'Periodicity' },
        { name: 'UnitMeasure' },
        { name: 'EbirModule' },
        { name: 'Municipality' },
        { name: 'Value' },
        { name: 'PeriodStart' },
        { name: 'Year' },
        { name: 'Month' },
        { name: 'NamePrepositional' }
    ]
});