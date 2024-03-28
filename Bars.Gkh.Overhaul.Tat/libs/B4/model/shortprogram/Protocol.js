Ext.define('B4.model.shortprogram.Protocol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramProtocol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ShortObject' },
        { name: 'Contragent' },
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
        { name: 'File' }
    ]
});