﻿Ext.define('B4.model.PropertyOwnerProtocols', {
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
        { name: 'Description' },
        { name: 'NumberOfVotes' },
        { name: 'TotalNumberOfVotes' },
        { name: 'PercentOfParticipating' },
        { name: 'TypeProtocol', defaultValue: 0 },
        { name: 'DocumentFile', defaultValue: null },
        { name: 'LoanAmount' },
        { name: 'Borrower' },
        { name: 'Lender' },
        { name: 'FormVoting' },
        { name: 'EndDateDecision' },
        { name: 'PlaceDecision' },
        { name: 'PlaceMeeting' },
        { name: 'DateMeeting' },
        { name: 'TimeMeeting', defaultValue: null },
        { name: 'VotingStartDate' },
        { name: 'VotingStartTime', defaultValue: null },
        { name: 'VotingEndDate' },
        { name: 'VotingEndTime', defaultValue: null }, 
        { name: 'OrderTakingDecisionOwners' }, 
        { name: 'OrderAcquaintanceInfo' }, 
        { name: 'AnnuaLMeeting' },
        { name: 'LegalityMeeting' },
        { name: 'VotingStatus' },

        { name: 'NpaName' },
        { name: 'NpaDate' },
        { name: 'NpaNumber' },
        { name: 'NpaStatus' },
        { name: 'NpaCancellationReason' },
        { name: 'TypeInformationNpa', defaultValue: null },
        { name: 'TypeNpa', defaultValue: null },
        { name: 'TypeNormativeAct', defaultValue: null },
        { name: 'NpaContragent', defaultValue: null },
        { name: 'NpaFile', defaultValue: null }
    ]
});