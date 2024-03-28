Ext.define('B4.store.contragent.ContragentForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    storeId: 'contragentForSelectedStore',
    model: 'B4.model.Contragent'
});