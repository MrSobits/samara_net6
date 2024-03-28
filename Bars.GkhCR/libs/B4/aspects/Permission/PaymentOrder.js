Ext.define('B4.aspects.permission.PaymentOrder', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.paymentorderperm',

    permissions: [
        { name: 'GkhCr.PaymentOrder.Create', applyTo: 'b4addbutton', selector: 'paymentordergrid' },
        { name: 'GkhCr.PaymentOrder.Edit', applyTo: 'b4savebutton', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Delete', applyTo: 'b4deletecolumn', selector: 'paymentordergrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.PaymentOrder.Field.BankStatement_Edit', applyTo: '#sfBankStatement', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.FinanceSource_Edit', applyTo: '#sfFinanceSource', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.PayerContragent_Edit', applyTo: '#sfPayerContragent', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.DocumentNum_Edit', applyTo: '#tfDocumentNum', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.BidNum_Edit', applyTo: '#tfBidNum', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.Sum_Edit', applyTo: '#nfSum', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.ReceiverContragent_Edit', applyTo: '#sfReceiverContragent', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.PayPurpose_Edit', applyTo: '#tfPayPurpose', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.DocumentDate_Edit', applyTo: '#dfDocumentDate', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.BidDate_Edit', applyTo: '#dfBidDate', selector: '#paymentOrderEditWindow' },
        { name: 'GkhCr.PaymentOrder.Field.RedirectFunds_Edit', applyTo: '#nfRedirectFunds', selector: '#paymentOrderEditWindow' }

    ]
});