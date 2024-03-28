Ext.define('B4.store.SEStateStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.State'],
    autoLoad: false,
    model: 'B4.model.State',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActualizeDPKR',
        listAction: 'ListSEStates'
    }
});