Ext.define('B4.store.regop.paymentdocument.PaymentDocumentLogsStore', {
    extend: 'B4.base.Store',
    autoLoad: true,
    fields: [
        { name: 'Id' },
        //{ name: 'ParentId' },
        { name: 'Uid' },
        { name: 'StartTime' },
        { name: 'Description' },
        { name: 'Count' },
        { name: 'AllCount' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentDocumentLog'
    }
});