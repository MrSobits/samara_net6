Ext.define('B4.model.multipleAnalysis.MunicipalityFias', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MultipleAnalysisTemplate',
        listAction: 'ListFiasArea'
    },
    fields: [
        { name: 'Id', mapping: 'FiasId' },
        { name: 'Name' }
    ]
});