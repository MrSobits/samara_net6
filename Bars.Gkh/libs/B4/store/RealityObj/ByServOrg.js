/**
* стор жилых домов управляющей организации, которыми уо может оперировать в системе
*/
Ext.define('B4.store.realityobj.ByServOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.servorg.RealityObject'],
    autoLoad: false,
    storeId: 'realityObjectByServOrgStore',
    model: 'B4.model.servorg.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListByServOrg'
    }
});