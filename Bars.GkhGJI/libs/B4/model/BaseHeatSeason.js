Ext.define('B4.model.BaseHeatSeason', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseHeatSeason'
    },
    fields: [
        { name: 'HeatingSeason', defaultValue: null },
        { name: 'DisposalDocumentNumber' },
        { name: 'DisposalDocumentDate' },
        { name: 'CountViol' },
        { name: 'ResolutionName' }
    ]
});