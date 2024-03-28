Ext.define('B4.store.edolog.Requests', {
    extend: 'B4.base.Store',
    requires: ['B4.model.edolog.Requests'],
    autoLoad: false,
    storeId: 'logRequestsStore',
    model: 'B4.model.edolog.Requests',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LogRequests',
        listAction: 'ListLogRequests'
    }
});