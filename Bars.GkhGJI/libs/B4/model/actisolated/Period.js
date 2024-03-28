Ext.define('B4.model.actisolated.Period', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedPeriod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolated', defaultValue: null },
        { name: 'DateCheck' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'TimeStart' },
        { name: 'TimeEnd' }
    ]
});