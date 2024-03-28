Ext.define('B4.store.import.chesimport.RecalcSummary', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport',
        listAction: 'ListRecalcInfo'
    },
    fields: [
        { name: 'WalletType' },
        { name: 'Recalc' }
    ]
});