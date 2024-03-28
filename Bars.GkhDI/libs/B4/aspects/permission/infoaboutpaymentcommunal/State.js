Ext.define('B4.aspects.permission.infoaboutpaymentcommunal.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.infoaboutpaymentcommunalstateperm',

    permissions: [
        { name: 'GkhDi.DisinfoRealObj.InfoAboutPaymentCommunal.Edit', applyTo: 'b4savebutton', selector: '#infoAboutPaymentCommunalInlineGrid' },
        { name: 'GkhDi.DisinfoRealObj.InfoAboutPaymentCommunal.Edit', applyTo: 'b4savebutton', selector: '#infoAboutPaymentCommunalEditWindow' }
    ]
});