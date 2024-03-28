Ext.define('B4.model.supplyresourceorg.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SupplyResourceOrgMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SupplyResourceOrg', defaultValue: null },
        { name: 'Municipality', defaultValue: null }
    ]
});