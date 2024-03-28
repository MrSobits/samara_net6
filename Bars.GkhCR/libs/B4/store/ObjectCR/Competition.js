Ext.define('B4.store.objectcr.Competition', {
    extend: 'B4.base.Store',
    fields: ['Id', 'CompetitionId', 'CompetitionState', 'CompetitionNotifNumber', 'CompetitionNotifDate', 'LotContractNumber', 'LotContractDate', 'TypeWorks', 'ProtocolSignDate', 'Winner'],
    autoLoad: false,
    storeId: 'objectcrCompetitions',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'ListCompetitions'
    }
});