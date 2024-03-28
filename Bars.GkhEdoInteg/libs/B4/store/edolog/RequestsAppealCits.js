Ext.define('B4.store.edolog.RequestsAppealCits', {
    extend: 'B4.base.Store',
    requires: ['B4.model.edolog.RequestsAppealCits'],
    autoLoad: false,
    storeId: 'requestsAppealCitsStore',
    model: 'B4.model.edolog.RequestsAppealCits',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LogRequests',
        listAction: 'ListRequestsAppealCits'
    }
});