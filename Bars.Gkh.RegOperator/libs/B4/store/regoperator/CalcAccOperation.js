Ext.define('B4.store.regoperator.CalcAccOperation', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'OperationDate' },
        { name: 'Reason' },
        { name: 'Amount' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'CalcAccount',
        listAction: 'ListOperations',
        timeout: 4 * 60 * 1000
    }
});