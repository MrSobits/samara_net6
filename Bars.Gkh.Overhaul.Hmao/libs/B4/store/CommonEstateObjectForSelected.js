Ext.define('B4.store.CommonEstateObjectForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.CommonEstateObject'],
    autoLoad: false,
    storeId: 'commonEstateObjectForSelectedStore',
    model: 'B4.model.CommonEstateObject'
});