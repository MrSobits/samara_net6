Ext.define('B4.store.manorg.contract.Base', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.contract.Base'],
    autoLoad: false,
    storeId: 'manorgBaseContractStore',
    model: 'B4.model.manorg.contract.Base'
});