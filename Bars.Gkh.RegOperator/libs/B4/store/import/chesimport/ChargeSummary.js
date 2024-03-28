Ext.define('B4.store.import.chesimport.ChargeSummary', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport',
        listAction: 'ListChargeInfo'
    },
    fields: [
        { name: 'WalletType' },
        { name: 'SaldoIn' },
        { name: 'Charged' },
        { name: 'Paid' },
        { name: 'SaldoChange' },
        { name: 'Recalc' },
        { name: 'SaldoOut' }
    ]
});