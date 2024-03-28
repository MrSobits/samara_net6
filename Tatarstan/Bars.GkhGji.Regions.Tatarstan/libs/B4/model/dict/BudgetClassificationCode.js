Ext.define('B4.model.dict.BudgetClassificationCode', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BudgetClassificationCode'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Kbk' },
        { name: 'Municipalities' },
        { name: 'ArticleLaw' },
        { name: 'StartDate' },
        { name: 'EndDate' },
    ]
});