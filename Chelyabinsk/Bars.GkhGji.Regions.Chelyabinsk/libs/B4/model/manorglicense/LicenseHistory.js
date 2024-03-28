Ext.define('B4.model.manorglicense.LicenseHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseHistory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContragentName' },
        { name: 'ContragentINN' },
        { name: 'ManOrgLicense' },
        { name: 'LicNum' },
        { name: 'LicNumGJI' },
        { name: 'BlankFile' },
        { name: 'OrderFile' },
        { name: 'DateIssued' },
        { name: 'DateRegister' },
        { name: 'DisposalNumber' },
        { name: 'DateDisposal' }
    ]
});