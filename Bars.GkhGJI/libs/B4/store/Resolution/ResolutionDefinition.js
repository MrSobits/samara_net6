Ext.define('B4.store.resolution.ResolutionDefinition', {
    extend: 'B4.base.Store',
    requires: ['B4.model.resolution.Definition'],
    autoLoad: false,
    storeId: 'resolutionResolutionDefinitionStore',
    model: 'B4.model.resolution.Definition',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeOperations',
        listAction: 'GetListResolutionDefinition'
    },
});