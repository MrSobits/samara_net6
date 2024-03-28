Ext.define('B4.store.resolution.CourtPracticeDecision', {
    extend: 'B4.base.Store',
    requires: ['B4.model.resolution.Decision'],
    autoLoad: false,
    storeId: 'resolutionCourtPracticeDecisionStore',
    model: 'B4.model.resolution.Decision',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeOperations',
        listAction: 'GetListDecision'
    },
});