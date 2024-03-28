Ext.define('B4.model.regoperator.CalcAccount', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOpCalcAccount',
        timeout: 4 * 60 * 1000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContragentBankCrOrg' },
        { name: 'TotalOut', defaultValue: null },
        { name: 'TotalIncome', defaultValue: null },
        { name: 'BalanceIncome', defaultValue: null },
        { name: 'BalanceOut', defaultValue: null },
        { name: 'LastOperationDate', defaultValue: null },
        { name: 'CreditOrg', defaultValue: null },
        { name: 'RegOperator', defaultValue: null },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'AccountType', defaultValue: 40 },
        { name: 'CreditLimit' },
        { name: 'IsSpecial' },
        { name: 'ContragentId' }
    ]
});