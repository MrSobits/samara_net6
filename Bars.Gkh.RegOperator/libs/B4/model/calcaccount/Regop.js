Ext.define('B4.model.calcaccount.Regop', {
    extend: 'B4.model.CalcAccount',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegopCalcAccount',
        timeout: 300000 // 5 минут
    },
    fields: [
        { name: 'ContragentCreditOrg', defaultValue: null },
        { name: 'ContragentAccountNumber', defaultValue: null },
        { name: 'OverdraftLimit' },
        { name: 'IsTransit' },
        { name: 'LastOperationDate', defaultValue: null }
    ]
});