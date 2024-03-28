Ext.define('B4.model.dict.FinanceSourceWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinanceSourceWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'FinanceSource', defaultValue: null },
        { name: 'Work', defaultValue: null },
        { name: 'WorkName' }
    ]
});