Ext.define('B4.model.smev.GISGMPPayments', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GISGMPPayments'
    },
    fields: [
        { name: 'Id'},
        { name: 'GisGmp' },
        { name: 'Amount' },
        { name: 'Kbk' },
        { name: 'OKTMO' },
        { name: 'PaymentDate' },
        { name: 'Purpose' },
        { name: 'SupplierBillID' },
        { name: 'PaymentId' },
        { name: 'Reconcile', defaultValue: 20 },
        { name: 'FileInfo' }
    ]
});