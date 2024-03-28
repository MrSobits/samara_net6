Ext.define('B4.model.integrationtor.TorTask', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TorTask'
    },
    fields: [
        { name: 'SendObject' },
        { name: 'TypeRequest' },
        { name: 'TaskState' },
        { name: 'TorId' },
        { name: 'RegistrationTorDate' },
        { name: 'RequestFileId' },
        { name: 'ResponseFileId' },
        { name: 'LogFileId' }
    ]
});