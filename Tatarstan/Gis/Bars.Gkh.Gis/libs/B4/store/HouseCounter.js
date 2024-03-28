Ext.define('B4.store.HouseCounter', {
    extend: 'B4.base.Store',
    requires: ['B4.model.HouseCounter'],
    autoLoad: false,
    storeId: 'houseCounterStore',
    model: 'B4.model.HouseCounter'
});