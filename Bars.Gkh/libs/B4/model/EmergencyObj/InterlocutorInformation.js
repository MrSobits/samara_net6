Ext.define('B4.model.emergencyobj.InterlocutorInformation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InterlocutorInformation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ApartmentNumber', defaultValue: null },
        { name: 'ApartmentArea', defaultValue: null },
        { name: 'FIO', defaultValue: null },
        { name: 'PropertyType', defaultValue: null },
        { name: 'AvailabilityMinorsAndIncapacitatedProprietors', defaultValue: null },
        { name: 'DateDemolitionIssuing', defaultValue: null },
        { name: 'DateDemolitionReceipt', defaultValue: null },
        { name: 'DateNotification', defaultValue: null },
        { name: 'DateNotificationReceipt', defaultValue: null },
        { name: 'DateAgreement', defaultValue: null },
        { name: 'DateAgreementRefusal', defaultValue: null },
        { name: 'DateOfReferralClaimCourt', defaultValue: null },
        { name: 'DateOfDecisionByTheCourt', defaultValue: null },
        { name: 'ConsiderationResultClaim', defaultValue: null },
        { name: 'DateAppealDecision', defaultValue: null },
        { name: 'AppealResult', defaultValue: null }
    ]
});