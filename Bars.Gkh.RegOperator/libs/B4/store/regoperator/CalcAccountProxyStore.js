Ext.define('B4.store.regoperator.CalcAccountProxyStore', {
    extend: 'B4.base.Store',
    fields: [
        { name: 'id' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'CreditOrg' },
        { name: 'Credit' },
        { name: 'Debt' },
        { name: 'Saldo' },
        { name: 'PercentSum' },
        { name: 'ContragentBank' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegopCalcAccountRealityObject',
        listAction: 'ListAccounts'
    }
});