Ext.define('B4.store.resolution.AppealDecision', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.Decision'],
    autoLoad: false,
    storeId: 'resolutionAppealDecisionStore',
    model: 'B4.model.appealcits.Decision',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeOperations',
        listAction: 'GetListAppealDecision'
    },
});