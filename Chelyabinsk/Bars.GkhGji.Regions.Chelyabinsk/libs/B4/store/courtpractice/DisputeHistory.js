Ext.define('B4.store.courtpractice.DisputeHistory', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.courtpractice.DisputeHistory'],
    storeId: 'disputeHistoryStore',
    model: 'B4.model.courtpractice.DisputeHistory'
});