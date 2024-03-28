Ext.define('B4.store.view.DisposalNullInspection', {
    requires: ['B4.model.Disposal'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewDisposalNullInspectionStore',
    model: 'B4.model.Disposal',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Disposal',
        listAction: 'ListNullInspection'
    }
});