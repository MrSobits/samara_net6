Ext.define('B4.store.House', {
    extend: 'B4.base.Store',
    requires: ['B4.model.House'],
    autoLoad: true,
    storeId: 'houseStore',
    model: 'B4.model.House'
});