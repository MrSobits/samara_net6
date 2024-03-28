Ext.define('B4.model.manorglicense.LicenseGis', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseGis',
        listAction: 'GetListWithRO',
        timeout:100000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'ContragentMunicipality' },
        { name: 'Request' },
        { name: 'LicNumber' },
        { name: 'LicNum' },
        { name: 'LicNumGJI' },
        { name: 'BlankFile' },
        { name: 'OrderFile' },
        { name: 'DateIssued' },
        { name: 'DateRegister' },
        { name: 'DisposalNumber' },
        { name: 'DateDisposal' },
        { name: 'DateTermination' },
        { name: 'State' },
        { name: 'TypeTermination', defaultValue: 0 },
        { name: 'ROMunicipality' },
        { name: 'ROAddress' },
        { name: 'ManStartDate' }, 
        { name: 'DateRegister' }, 
        { name: 'MkdCount' }, 
        { name: 'StateName' }, 
        { name: 'MKDArea' },
        { name: 'DisposalNumber' },
        { name: 'DateDisposal' },
        { name: 'ManEndDate' },
        { name: 'Inn' },
        { name: 'postCount' },
        { name: 'mcId' }
    ]
});