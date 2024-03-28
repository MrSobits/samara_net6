Ext.define('B4.store.contragent.Contact', {
    extend: 'B4.base.Store',
    requires: ['B4.model.contragent.Contact'],
    autoLoad: false,
    storeId: 'contragentContactStore',
    model: 'B4.model.contragent.Contact'
});