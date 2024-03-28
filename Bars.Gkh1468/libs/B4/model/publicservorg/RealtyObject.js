Ext.define('B4.model.publicservorg.RealtyObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicServiceOrgRealtyObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PublicServiceOrg', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'FiasAddress' },
        { name: 'ManOrgs' },
        { name: 'Address' }
    ]
});