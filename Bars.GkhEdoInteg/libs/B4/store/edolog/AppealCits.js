Ext.define('B4.store.edolog.AppealCits', {
    extend: 'B4.base.Store',
    requires: ['B4.model.edolog.AppealCits'],
    autoLoad: false,
    storeId: 'appealCitsLogStore',
    model: 'B4.model.edolog.AppealCits',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EdoIntegration',
        listAction: 'ListAppealCitsLog'
    }
});