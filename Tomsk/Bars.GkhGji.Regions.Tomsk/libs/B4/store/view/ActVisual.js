Ext.define('B4.store.view.ActVisual', {
    requires: ['B4.model.ActVisual'],
    extend: 'B4.base.Store',
    autoLoad: false,

    model: 'B4.model.ActVisual',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActVisual',
        listAction: 'ListView'
    }
});