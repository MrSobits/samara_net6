Ext.define('B4.store.AppealCitsEdoInteg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppealCits'],
    autoLoad: false,
    storeId: 'appealCitsEdoIntegStore',
    model: 'B4.model.AppealCits',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EdoIntegration',
        listAction: 'ListAppealCits'
    }
});