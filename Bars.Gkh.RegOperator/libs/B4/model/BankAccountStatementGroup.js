Ext.define('B4.model.BankAccountStatementGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BankAccountStatementGroup'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ImportDate' },
        { name: 'UserLogin' },
        { name: 'DocumentDate' },
        { name: 'Sum' }
    ]
});