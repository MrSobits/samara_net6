Ext.define('B4.store.import.chesimport.SaldoChangeSummary', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport',
        listAction: 'ListSaldoChangeInfo'
    },
    fields: [
        { name: 'WalletType' },
        { name: 'Change' }
    ]
});