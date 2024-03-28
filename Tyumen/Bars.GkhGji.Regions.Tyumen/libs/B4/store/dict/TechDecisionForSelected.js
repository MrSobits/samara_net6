Ext.define('B4.store.dict.TechDecisionForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.TechDecision'],
    autoLoad: false,
    storeId: 'techDecisionForSelectedStore',
    model: 'B4.model.dict.TechDecision'
});