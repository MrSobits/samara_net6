Ext.define('B4.store.objectcr.ObjectCrBuilder', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Builder'],
    autoLoad: false,
    storeId: 'objectCrBuilderStore',
    model: 'B4.model.Builder',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'GetBuilders'
    }
});