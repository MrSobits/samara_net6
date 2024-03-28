Ext.define('B4.model.TransferRf', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferRf'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContractRf', defaultValue: null },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'ManagingOrganizationName' },
        { name: 'MunicipalityName' },
        { name: 'ContractRfObjectsCount' }
        
    ]
});