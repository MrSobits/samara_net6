Ext.define('B4.model.supplyresourceorg.RealtyObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SupplyResourceOrgRealtyObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealObjId', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'SupplyResourceOrg', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'FiasAddress' },
        { name: 'Address' }
    ]
});