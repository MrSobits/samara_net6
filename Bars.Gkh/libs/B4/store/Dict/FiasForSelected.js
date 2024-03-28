Ext.define('B4.store.dict.FiasForSelected', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.Fias'],
    storeId: 'fiasStreetForSelectedStore',
    model: 'B4.model.Fias',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Fias',
        listAction: 'ListMo'
    }
});