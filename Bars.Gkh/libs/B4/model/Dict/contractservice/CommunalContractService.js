Ext.define('B4.model.dict.contractservice.CommunalContractService', {
    extend: 'B4.model.dict.contractservice.ManagementContractService',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'communalContractService'
    },
    fields: [
        { name: 'CommunalResource' },
        { name: 'SortOrder' },
        { name: 'IsHouseNeeds', defaultValue: false }
    ]
});