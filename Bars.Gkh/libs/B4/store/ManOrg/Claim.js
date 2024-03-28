Ext.define('B4.store.manorg.Claim', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.Claim'],
    autoLoad: false,
    storeId: 'manorgClaimStore',
    model: 'B4.model.manorg.Claim'
});