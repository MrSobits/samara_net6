Ext.define('B4.model.manorglicense.LicensePerson', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicensePerson'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'License' },
        { name: 'Person' },
        { name: 'PersonFullName' },
        { name: 'Position' }
    ]
});