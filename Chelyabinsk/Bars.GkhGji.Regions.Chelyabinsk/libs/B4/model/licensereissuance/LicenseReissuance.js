Ext.define('B4.model.licensereissuance.LicenseReissuance', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LicenseReissuance'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'ReissuanceDate' },
        { name: 'RegisterNumber' },
        { name: 'RegisterNum' },
        { name: 'ManOrgLicense' },
        { name: 'ConfirmationOfDuty' },
        { name: 'State' },
        { name: 'OfficialsCount', defaultValue: null }
    ]
});