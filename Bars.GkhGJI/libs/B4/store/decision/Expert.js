Ext.define('B4.store.decision.Expert', {
    extend: 'B4.base.Store',
    requires: ['B4.model.decision.Expert'],
    autoLoad:false,
    storeId: 'decisionExpertStore',
    model: 'B4.model.decision.Expert'
});