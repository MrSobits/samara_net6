Ext.define('B4.aspects.permission.infoaboutpaymenthousing.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.infoaboutpaymenthousingstateperm',

    permissions: [
        { name: 'GkhDi.DisinfoRealObj.InfoAboutPaymentHousing.Edit', applyTo: 'b4savebutton', selector: '#infoAboutPaymentHousingGrid' }
    ]
});