/**
* стор жилых домов управляющей организации, которыми уо может оперировать в системе
*/
Ext.define('B4.store.realityobj.ByManOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.RealityObject'],
    autoLoad: false,
    storeId: 'realityObjectByManOrgStore',
    model: 'B4.model.manorg.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListByManOrg'
    }
});