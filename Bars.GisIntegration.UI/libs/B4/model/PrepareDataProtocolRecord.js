Ext.define('B4.model.PrepareDataProtocolRecord', {
    extend: 'B4.base.Model',
    idgen: {
        type: 'sequential',
        prefix: 'ID_'
    },
    idProperty: undefined,
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskTree',
        listAction: 'GetTriggerProtocol'
    },
    fields: [
        { name: 'DateTime' },
        { name: 'Type' },
        { name: 'Text' }
    ]
});
