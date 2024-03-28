Ext.define('B4.model.emergencyobj.Documents', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmergencyObjectDocuments'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EmergencyObject', defaultValue: null },
        { name: 'RequirementPublicationDate' },
        { name: 'RequirementDocumentNumber', defaultValue: null },
        { name: 'RequirementFile', defaultValue: null },
        { name: 'DecreePublicationDate' },
        { name: 'DecreeRequisitesMpa', defaultValue: null },
        { name: 'DecreeMpaPublicationDate', defaultValue: null },
        { name: 'DecreeMpaRegistrationDate', defaultValue: null },
        { name: 'DecreeMpaNotificationDate', defaultValue: null },
        { name: 'DecreeFile', defaultValue: null },
        { name: 'AgreementPublicationDate' },
        { name: 'AgreementFile', defaultValue: null }
    ]
});