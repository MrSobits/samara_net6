Ext.define('B4.model.GisGmpPatternDict', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGmpPatternDict'
    },
    fields: [
        { name: 'PatternName' },
        { name: 'PatternCode' },
        { name: 'Relevance' }
    ]
});