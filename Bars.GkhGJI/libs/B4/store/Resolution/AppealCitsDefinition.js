Ext.define('B4.store.resolution.AppealCitsDefinition', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.Definition'],
    autoLoad: false,
    storeId: 'resolutionAppealCitsDefinitionStore',
    model: 'B4.model.appealcits.Definition',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeOperations',
        listAction: 'GetListAppealCitsDefinition'
    },
});