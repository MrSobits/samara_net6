/**
* стор жилых домов управляющей организации, которыми уо может оперировать в системе
*/
Ext.define('B4.store.realityobj.ByManOrgAndContractDate', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.RealityObject'],
    autoLoad: false,
    model: 'B4.model.manorg.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractRf',
        listAction: 'ListByManOrgAndContractDate'
    }
});