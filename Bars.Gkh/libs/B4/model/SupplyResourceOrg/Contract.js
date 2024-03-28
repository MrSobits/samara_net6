Ext.define('B4.model.supplyresourceorg.Contract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectResOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ResourceOrg', defaultValue: null },
        { name: 'ResourceOrgId' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address', defaultValue: null },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'ContractNumber' },
        { name: 'ContractDate' },
        { name: 'Note' },
        { name: 'FileInfo', defaultValue: null }
    ]
});