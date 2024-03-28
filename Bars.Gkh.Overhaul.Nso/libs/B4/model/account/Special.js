Ext.define('B4.model.account.Special', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Number', defaultValue: null },
        { name: 'OpenDate', defaultValue: null },
        { name: 'CloseDate', defaultValue: null },
        { name: 'TotalIncome', defaultValue: 0 },
        { name: 'TotalOut', defaultValue: 0 },
        { name: 'Balance', defaultValue: 0 },
        { name: 'LastOperationDate', defaultValue: null },
        { name: 'CreditOrganization', defaultValue: null },
        { name: 'AccountOwner', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'AccountType', defaultValue: 20 },
        { name: 'Decision', defaultValue: null }
    ]
});