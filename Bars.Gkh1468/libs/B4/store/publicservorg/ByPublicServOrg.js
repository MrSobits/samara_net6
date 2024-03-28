/**
* стор жилых домов Поставщика ресурсов, которыми он может оперировать в системе
*/
Ext.define('B4.store.publicservorg.ByPublicServOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.publicservorg.RealtyObject'],
    autoLoad: false,
    storeId: 'realObjByPublicServOrgStore',
    model: 'B4.model.publicservorg.RealtyObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject1468',
        listAction: 'ListByPublicServOrg'
    }
});