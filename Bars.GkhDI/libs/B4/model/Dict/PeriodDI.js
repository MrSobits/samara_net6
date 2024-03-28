Ext.define('B4.model.dict.PeriodDi', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PeriodDi'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'DateAccounting', defaultValue: null },
        { name: 'Name' }
    ]
});