Ext.define('B4.store.suggestion.Rubric', {
    extend: 'B4.base.Store',
    requires: ['B4.model.suggestion.Rubric'],
    autoLoad: false,
    storeId: 'rubricStore',
    model: 'B4.model.suggestion.Rubric'
});