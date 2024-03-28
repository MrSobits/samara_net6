Ext.define('B4.model.licensereissuance.LicenseReissuancePerson', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LicenseReissuancePerson'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LicenseReissuance' },
        { name: 'Person' },
        { name: 'PersonFullName' },
        { name: 'Position' }
    ]
});