Ext.define('B4.store.presentation.Relations', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.DocumentGjiChildren'],
    storeId: 'presentationRelationsStore',
    model: 'B4.model.DocumentGjiChildren'
});