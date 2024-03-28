Ext.define('B4.store.contragent.ForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.contragent.Contragent'],
    autoLoad: false,
    storeId: 'ForSelectedStore',
    model: 'B4.model.contragent.Contragent'
});