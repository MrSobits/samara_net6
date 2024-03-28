Ext.define('B4.model.supplyresourceorg.Documentation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SupplyResourceOrgDocumentation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SupplyResourceOrg', defaultValue: null },
        { name: 'Description' },
        { name: 'DocumentName' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'File', defaultValue: null }
    ]
});