Ext.define('B4.store.protocolgji.Relations', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.DocumentGjiChildren'],
    storeId: 'protocolRelationsStore',
    model: 'B4.model.DocumentGjiChildren'
});