Ext.define('B4.store.view.Presentation', {
    requires: ['B4.model.Presentation'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewPresentationStore',
    model: 'B4.model.Presentation',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Presentation',
        listAction: 'ListView'
    }
});