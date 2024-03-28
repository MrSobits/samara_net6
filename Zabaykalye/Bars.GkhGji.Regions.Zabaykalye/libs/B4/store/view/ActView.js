Ext.define('B4.store.view.ActView', {
    requires: ['B4.model.ActCheck'],
    extend: 'B4.base.Store',
    autoLoad: false,
    model: 'B4.model.ActCheck',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheck',
        listAction: 'ListView'
    }
});