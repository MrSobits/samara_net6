Ext.define('B4.model.manorglicense.License', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicense',
        listAction: 'ListForRequestType'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'ContragentMunicipality' },
        { name: 'ShortName' },
        { name: 'Inn' },
        { name: 'Request' },
        { name: 'LicNumber' },
        { name: 'LicNum' },
        { name: 'DateIssued' },
        { name: 'DateRegister' },
        { name: 'DisposalNumber' },
        { name: 'DateDisposal' },
        { name: 'DateTermination' },
        { name: 'State' },
        { name: 'TypeTermination', defaultValue: 0 },
        { name: 'HousingInspection' },
        { name: 'OrganizationMadeDecisionTermination' },
        { name: 'DocumentTermination' },
        { name: 'DocumentNumberTermination' },
        { name: 'DocumentDateTermination',  defaultValue: null},
        { name: 'TerminationFile' },
        { name: 'TypeIdentityDocument' },
        { name: 'IdSerial' },
        { name: 'IdNumber' },
        { name: 'IdIssuedBy' },
        { name: 'IdIssuedDate' }
    ]
});