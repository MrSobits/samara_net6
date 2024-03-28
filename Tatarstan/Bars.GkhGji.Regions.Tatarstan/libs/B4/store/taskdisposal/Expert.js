Ext.define('B4.store.taskdisposal.Expert', {
    extend: 'B4.base.Store',
    requires: ['B4.model.disposal.Expert'],
    autoLoad: false,
    storeId: 'taskdisposalExpertStore',
    model: 'B4.model.disposal.Expert'
});