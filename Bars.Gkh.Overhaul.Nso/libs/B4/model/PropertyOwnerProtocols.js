Ext.define('B4.model.PropertyOwnerProtocols', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PropertyOwnerProtocols'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'TypeProtocol', defaultValue: 0 },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'Description' },
        { name: 'NumberOfVotes' },
        { name: 'TotalNumberOfVotes' },
        { name: 'PercentOfParticipating' },
        { name: 'DocumentFile', defaultValue: null }
    ]
});