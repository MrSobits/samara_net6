Ext.define('B4.model.manorglicense.License', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicense'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'ContragentMunicipality' },
        { name: 'Request' },
        { name: 'LicNumber' },
        { name: 'LicNum' },
        { name: 'Inn' },
        { name: 'Ogrn' },
        { name: 'DateIssued' },
        { name: 'DateValidity' },
        { name: 'DateRegister' },
        { name: 'DisposalNumber' },
        { name: 'DateDisposal' },
        { name: 'ERULNumber' },
        { name: 'ERULDate' },
        { name: 'DateTermination' },
        { name: 'State' },
        { name: 'ContragentState' },
        { name: 'TypeTermination', defaultValue: 0 }
    ]
});