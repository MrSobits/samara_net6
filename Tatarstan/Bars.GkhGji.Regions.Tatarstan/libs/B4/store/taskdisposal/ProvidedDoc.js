Ext.define('B4.store.taskdisposal.ProvidedDoc', {
    extend: 'B4.base.Store',
    requires: ['B4.model.disposal.ProvidedDoc'],
    autoLoad:false,
    storeId: 'taskdisposalProvidedDocStore',
    model: 'B4.model.disposal.ProvidedDoc'
});