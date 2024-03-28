Ext.define('B4.model.actremoval.Period', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActRemovalPeriod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActRemoval', defaultValue: null },
        { name: 'DateCheck' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'TimeStart' },
        { name: 'TimeEnd' }
    ]
});