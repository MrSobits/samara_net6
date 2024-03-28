Ext.define('B4.store.manorg.Membership', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.Membership'],
    autoLoad: false,
    storeId: 'manorgMembershipStore',
    model: 'B4.model.manorg.Membership'
});