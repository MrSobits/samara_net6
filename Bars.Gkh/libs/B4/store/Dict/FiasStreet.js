Ext.define('B4.store.dict.FiasStreet', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.Fias'],
    storeId: 'fiasMoStore',
    model: 'B4.model.Fias',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Fias',
        listAction: 'ListMo'
    }
});