Ext.define('B4.model.supplyresourceorg.Service', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SupplyResourceOrgService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SupplyResourceOrg', defaultValue: null },
        { name: 'TypeService' }
    ]
});