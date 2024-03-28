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
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        //***//
        { name: 'VoteForm', defaultValue: 10 },
        { name: 'Municipality' },
        { name: 'RegistrationDate' },
        { name: 'Inspector' },
        { name: 'RegistrationNumber' },
        { name: 'ProtocolMKDState' },
        { name: 'ProtocolMKDSource' },
        { name: 'ProtocolMKDIniciator' },
        //***//
        { name: 'Description' },
        { name: 'NumberOfVotes' },
        { name: 'TotalNumberOfVotes' },
        { name: 'PercentOfParticipating' },
        { name: 'ProtocolTypeId' },
        { name: 'DocumentFile', defaultValue: null }
    ]
});