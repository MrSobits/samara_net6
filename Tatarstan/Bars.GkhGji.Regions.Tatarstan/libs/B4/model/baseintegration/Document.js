// Используется в качестве базового
Ext.define('B4.model.baseintegration.Document', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'DocumentNumber' },
        { name: 'DocumentDate'},
        { name: 'DocumentType' },
        { name: 'DocumentTypeBase' },
        { name: 'InspectionId' },
        { name: 'LastMethodStartTime' }
    ]
});