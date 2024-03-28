Ext.define('B4.aspects.permission.infoaboutreductionpayment.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.infoaboutreductionpaymentstateperm',

    permissions: [
        { name: 'GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Add', applyTo: '#addInfoAboutReductionPaymentButton', selector: '#infoAboutReductionPaymentEditPanel' },
        { name: 'GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Edit', applyTo: '#infoAboutReductionPaymentSaveButton', selector: '#infoAboutReductionPaymentEditPanel' },
        { name: 'GkhDi.DisinfoRealObj.InfoAboutReductionPayment.PaymentReductionField', applyTo: '#cbReductionPayment', selector: '#infoAboutReductionPaymentGridPanel' },
        {
            name: 'GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Delete', applyTo: 'b4deletecolumn', selector: '#infoAboutReductionPaymentGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});