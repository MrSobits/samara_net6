Ext.define('B4.store.view.ActIsolated', {
    requires: ['B4.model.ActIsolated'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewActIsolatedStore',
    model: 'B4.model.ActIsolated',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolated',
        listAction: 'ListView'
    }
});