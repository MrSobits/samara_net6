Ext.define('B4.aspects.permission.terminatecontract.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.terminatecontractstateperm',

    permissions: [
        { name: 'GkhDi.Disinfo.TerminateContract.TerminateContractField', applyTo: '#cbTerminateContract', selector: '#terminateContractEditPanel' },
        { name: 'GkhDi.Disinfo.TerminateContract.ManagingRealityObjBtn', applyTo: '#realityObjButton', selector: '#terminateContractEditPanel' }
    ]
});