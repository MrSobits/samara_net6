Ext.define('B4.model.person.RequestToExam', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonRequestToExam'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Person' },
        { name: 'RequestSupplyMethod' },
        { name: 'RequestNum' },
        { name: 'RequestDate' },
        { name: 'RequestTime' },
        { name: 'RequestFile' },
        { name: 'PersonalDataConsentFile' },
        { name: 'NotificationNum' },
        { name: 'NotificationDate' },
        { name: 'IsDenied' },
        { name: 'ExamDate' },
        { name: 'ExamTime' },
        { name: 'CorrectAnswersPercent' },
        { name: 'ProtocolNum' },
        { name: 'ProtocolDate' },
        { name: 'ProtocolFile' },
        { name: 'ResultNotificationNum' },
        { name: 'ResultNotificationDate' },
        { name: 'MailingDate' },
        { name: 'State' },
        { name: 'HasCert' },
        { name: 'FullName' }
    ]
});