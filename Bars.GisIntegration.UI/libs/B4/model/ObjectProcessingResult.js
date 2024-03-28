Ext.define('B4.model.ObjectProcessingResult', {
    extend: 'B4.base.Model',
    idgen: {
        type: 'sequential',
        prefix: 'ID_'
    },
    idProperty: undefined,
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskTree',
        listAction: 'GetObjectProcessingResults'
    },
    fields: [
        { name: 'PackageName'},
        { name: 'RisId' },
        { name: 'ExternalId' },
        { name: 'GisId' },
        { name: 'Description' },
        { name: 'State' },
        { name: 'Message' }
    ]
});
