Ext.define('B4.store.specialobjectcr.Competition', {
    extend: 'B4.base.Store',
    fields: [
        'Id',
        'CompetitionId',
        'CompetitionState',
        'CompetitionNotifNumber',
        'CompetitionNotifDate',
        'LotContractNumber',
        'LotContractDate',
        'TypeWorks',
        'ProtocolSignDate',
        'Winner'
    ],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialObjectCr',
        listAction: 'ListCompetitions'
    }
});