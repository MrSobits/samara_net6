/**
* стор жилых домов, направлен на метод, возвращающий список домов по типу юр.лица
*/
Ext.define('B4.store.realityobj.ByTypeOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'realityObjectByManOrgStore',
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListByTypeOrg'
    }
});