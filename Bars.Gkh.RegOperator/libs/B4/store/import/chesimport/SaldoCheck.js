Ext.define('B4.store.import.chesimport.SaldoCheck', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport',
        listAction: 'ListSaldoCheck',
        timeout: 1000 * 60 * 5
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'LsNum' },
        { name: 'ChesLsNum' },
        { name: 'Saldo' },
        { name: 'ChesSaldo' },
        { name: 'DiffSaldo' },
        { name: 'IsImported' }
    ]
});