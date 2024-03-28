Ext.define('B4.store.publicservorg.RealityObjectInContract', {
    extend: 'B4.base.Store',
    requires: ['B4.model.publicservorg.PublicServiceOrgContractRealObj'],
    autoLoad: false,
    model: 'B4.model.publicservorg.PublicServiceOrgContractRealObj',
    storeId: 'publicServOrgContractRealityObjectStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicServiceOrgContractRealObj',
        listAction: 'ListByPublicServOrgContract'
    }
});