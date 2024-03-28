Ext.define('B4.model.DisclosureInfoLicense', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisclosureInfoLicense'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'LicenseNumber', defaultValue: null },
        { name: 'DateReceived', defaultValue: null },
        { name: 'LicenseOrg', defaultValue: null },
        { name: 'LicenseDoc', defaultValue: null }
    ]
});