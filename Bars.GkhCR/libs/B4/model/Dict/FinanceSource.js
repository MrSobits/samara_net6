Ext.define('B4.model.dict.FinanceSource', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinanceSource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'TypeFinanceGroup', defaultValue: 10 },
        { name: 'TypeFinance', defaultValue: 10 }
    ]
});