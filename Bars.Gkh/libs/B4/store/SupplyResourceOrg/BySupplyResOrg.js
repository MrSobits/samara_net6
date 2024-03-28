/**
* стор жилых домов Поставщика коммунальных услуг, которыми он может оперировать в системе
*/
Ext.define('B4.store.supplyresourceorg.BySupplyResOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.supplyresourceorg.RealtyObject'],
    autoLoad: false,
    storeId: 'realObjBySupSerOrgStore',
    model: 'B4.model.supplyresourceorg.RealtyObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListBySupplySerOrg'
    }
});