Ext.define('B4.store.decision.ProvidedDoc', {
    extend: 'B4.base.Store',
    requires: ['B4.model.decision.ProvidedDoc'],
    autoLoad:false,
    storeId: 'decisionProvidedDocStore',
    model: 'B4.model.decision.ProvidedDoc'
});