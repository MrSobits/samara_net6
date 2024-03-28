Ext.define('B4.aspects.permission.informationoncontracts.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.informationoncontractsstateperm',

    permissions: [
        { name: 'GkhDi.Disinfo.FundsInfo.FundsInfoField', applyTo: '#cbContractsAvailability', selector: '#informationOnContractsEditPanel' },
        { name: 'GkhDi.Disinfo.FundsInfo.Add', applyTo: '#addInformationOnContractsButton', selector: '#informationOnContractsEditPanel' },
        { name: 'GkhDi.Disinfo.FundsInfo.Edit', applyTo: 'b4savebutton', selector: '#informationOnContractsEditPanel' }
    ]
});