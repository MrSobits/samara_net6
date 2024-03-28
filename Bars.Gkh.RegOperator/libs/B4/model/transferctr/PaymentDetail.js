Ext.define('B4.model.transferctr.PaymentDetail', {
    extend: 'B4.base.Model',

    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferCtrPaymentDetail'
    },
    fields: [
        { name: 'WalletId' },
        { name: 'WalletName' },
        { name: 'Amount' },
        { name: 'Balance' },
        { name: 'RefundSum', defaultValue: 0 }
    ]
});