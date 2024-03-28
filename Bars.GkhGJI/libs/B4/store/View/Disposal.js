Ext.define('B4.store.view.Disposal', {
    requires: ['B4.model.Disposal'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewDisposalStore',
    model: 'B4.model.Disposal',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Disposal',
        listAction: 'ListView',
        timeout: 90000
    }
});