Ext.define('B4.store.resolution.PayFine', {
    extend: 'B4.base.Store',
    requires: ['B4.model.resolution.PayFine'],
    autoLoad:false,
    storeId: 'resolutionPayFineStore',
    model: 'B4.model.resolution.PayFine'
});