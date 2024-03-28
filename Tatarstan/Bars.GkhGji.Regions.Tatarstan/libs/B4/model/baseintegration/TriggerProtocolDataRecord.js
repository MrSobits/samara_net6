Ext.define('B4.model.baseintegration.TriggerProtocolDataRecord', {
    extend: 'B4.base.Model',
    idgen: {
        type: 'sequential',
        prefix: 'ID_'
    },
    idProperty: undefined,
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseIntegration',
        listAction: 'GetTriggerProtocolView'
    },
    fields: [
        { name: 'DateTime' },
        { name: 'Type' },
        { name: 'Text' }
    ]
});