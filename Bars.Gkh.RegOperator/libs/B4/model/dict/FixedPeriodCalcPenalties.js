Ext.define('B4.model.dict.FixedPeriodCalcPenalties', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FixedPeriodCalcPenalties'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'StartDay' },
        { name: 'EndDay' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});