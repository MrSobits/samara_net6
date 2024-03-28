Ext.define('B4.store.actcheck.ProvidedDoc', {
    extend: 'B4.base.Store',
    requires: ['B4.model.actcheck.ProvidedDoc'],
    autoLoad:false,
    storeId: 'actcheckProvidedDocStore',
    model: 'B4.model.actcheck.ProvidedDoc'
});