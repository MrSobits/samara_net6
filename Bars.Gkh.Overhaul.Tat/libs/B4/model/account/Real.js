Ext.define('B4.model.account.Real', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealAccount'
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
        { name: 'AccountOwner' },
        { name: 'OverdraftLimit' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'AccountType', defaultValue: 10}
    
    ]
});