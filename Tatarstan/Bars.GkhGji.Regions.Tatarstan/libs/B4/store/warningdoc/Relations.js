Ext.define('B4.store.warningdoc.Relations', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.DocumentGjiChildren'],
    storeId: 'warningdocRelationsStore',
    model: 'B4.model.DocumentGjiChildren'
});