Ext.define('B4.model.publicservorg.contractpart.IndividualOwnerContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'IndividualOwnerContract',
        writer: {
            type: 'b4writer',
            writeAllFields: true
        }
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PublicServiceOrgContract', defaultValue: null },
        { name: 'TypeContractPart', defaultValue: null },
        { name: 'TypeContactPerson', defaultValue: null },
        { name: 'TypeOwnerContract', defaultValue: null },
        { name: 'FirstName', defaultValue: null },
        { name: 'LastName', defaultValue: null },
        { name: 'MiddleName', defaultValue: null },
        { name: 'Gender', defaultValue: null },
        { name: 'OwnerDocumentType', defaultValue: null },
        { name: 'IssueDate', defaultValue: null },
        { name: 'DocumentSeries', defaultValue: null },
        { name: 'DocumentNumber', defaultValue: null },
        { name: 'BirthPlace', defaultValue: null },
        { name: 'BirthDate', defaultValue: null }
    ]
});