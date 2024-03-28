Ext.define('B4.store.view.ActRemoval', {
    requires: ['B4.model.ActRemoval'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewActRemovalStore',
    model: 'B4.model.ActRemoval',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActRemoval',
        listAction: 'ListView',
        timeout: 60000
    }
});