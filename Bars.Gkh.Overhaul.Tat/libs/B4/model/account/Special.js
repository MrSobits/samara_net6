Ext.define('B4.model.account.Special', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Number' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'TotalIncome' },
        { name: 'TotalOut' },
        { name: 'Balance' },
        { name: 'LastOperationDate' },
        { name: 'CreditOrganization' },
        { name: 'AccountOwner' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'AccountType', defaultValue: 20 }
    ]
});