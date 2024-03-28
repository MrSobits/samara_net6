Ext.define('B4.model.manorg.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrgRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'ActiveManagingOrganization' },
        { name: 'ActiveInn' },
        { name: 'ActiveDateStart' },
        { name: 'ActiveLicenseDate' }
    ]
});