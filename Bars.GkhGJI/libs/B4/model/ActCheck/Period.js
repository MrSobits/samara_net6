Ext.define('B4.model.actcheck.Period', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckPeriod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'DateCheck' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'TimeStart' },
        { name: 'TimeEnd' }
    ]
});