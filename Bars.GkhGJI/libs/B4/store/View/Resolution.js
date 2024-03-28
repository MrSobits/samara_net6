Ext.define('B4.store.view.Resolution', {
    requires: ['B4.model.Resolution'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewResolutionStore',
    model: 'B4.model.Resolution',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Resolution',
        listAction: 'ListView'
    }
});