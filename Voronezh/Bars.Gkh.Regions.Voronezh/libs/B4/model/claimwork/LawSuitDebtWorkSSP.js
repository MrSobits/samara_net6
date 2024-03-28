Ext.define('B4.model.claimwork.LawSuitDebtWorkSSP', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'LawSuitDebtWorkSSP'
    },

    fields: [
        { name: 'Id' },
        { name: 'Lawsuit' },
        { name: 'LawsuitOwnerInfo' },
        { name: 'CbDebtSum' },
        { name: 'CbPenaltyDebt' },
        { name: 'CbFactInitiated' },
        { name: 'FactInitiatedNote' },
        { name: 'CbDateInitiated' },
        { name: 'CbStationSsp' },
        { name: 'CbDateSsp' },
        { name: 'CbDocumentType' },
        { name: 'CbSumRepayment' },
        { name: 'CbDateDocument' },
        { name: 'CbNumberDocument' },
        { name: 'CbFile' },
        { name: 'CbSumStep' },
        { name: 'CbIsStopped' },
        { name: 'CbDateStopped' },
        { name: 'CbReasonStoppedType' },
        { name: 'CbReasonStoppedDescription' },
        { name: 'DateDirectionForSsp' },
        { name: 'CbSize' },
        
        //Новые поля для Смоленска
        {name: 'CbDebtDecisionSum'},
        {name: 'CbDuty'},
        {name: 'BtPayed'},
        {name: 'TRPayed'},
        {name: 'PenaltyPayed'},
        {name: 'DutyPayed'},
        {name: 'BtPayedForcibly'},
        {name: 'TRPayedForcibly'},
        {name: 'PenaltyPayedForcibly'},
        {name: 'DutyPayedForcibly'},
        {name: 'DebtWorkType'},
        {name: 'CbDocReturned'},
        {name: 'LackOfPropertyActDate'},
        {name: 'LackOfPropertyAct'}
        
    ]
});


