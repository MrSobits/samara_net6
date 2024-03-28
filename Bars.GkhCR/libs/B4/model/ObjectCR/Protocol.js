Ext.define('B4.model.objectcr.Protocol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'TypeWork' },
        { name: 'Contragent' },
        { name: 'ContragentName' },
        { name: 'TypeDocumentCr'},
        { name: 'DocumentName' },
        { name: 'DocumentNum' },
        { name: 'CountAccept' },
        { name: 'CountVote' },
        { name: 'CountVoteGeneral' },
        { name: 'Description' },
        { name: 'DateFrom' },
        { name: 'GradeOccupant' },
        { name: 'SumActVerificationOfCosts' },
        { name: 'GradeClientGrade' },
        { name: 'GradeClient' },
        { name: 'File' },
        { name: 'OwnerName' },
        { name: 'UsedInExport', defaultValue: 20 },
        { name: 'DecisionOms' }
    ]
});