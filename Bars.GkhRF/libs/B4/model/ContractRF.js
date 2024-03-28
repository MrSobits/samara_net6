Ext.define('B4.model.ContractRf', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractRf'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'ContragentName' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'DateBegin', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'MunicipalityName' },
        { name: 'ManagingOrganizationName' },
        { name: 'ContractRfObjectsCount' },
        { name: 'File', defaultValue: null },
        { name: 'SumAreaMkd' },
        { name: 'TerminationContractNum' },
        { name: 'TerminationContractDate' },
        { name: 'TerminationContractFile', defaultValue: null },
        { name: 'SumAreaLivingOwned' }
    ]
});