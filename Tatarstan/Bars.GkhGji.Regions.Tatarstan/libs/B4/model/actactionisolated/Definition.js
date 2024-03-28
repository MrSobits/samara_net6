Ext.define('B4.model.actactionisolated.Definition', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActActionIsolatedDefinition'
    },
    fields: [
        {name: 'Number'},
        {name: 'Date'},
        {name: 'DefinitionType'},
        {name: 'Official'},
        {name: 'ExecutionDate'},
        {name: 'RealityObject'},
        {name: 'Description'},
        {name: 'Act'}
    ]
});