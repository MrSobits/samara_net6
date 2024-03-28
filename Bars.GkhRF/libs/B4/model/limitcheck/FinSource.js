Ext.define('B4.model.limitcheck.FinSource', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LimitCheckFinSource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'FinanceSource', defaultValue: null },
        { name: 'LimitCheck', defaultValue: null }
    ]
});