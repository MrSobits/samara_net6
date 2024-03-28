Ext.define('B4.model.objectcr.ContractCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr', defaultValue: null },
        { name: 'FinanceSource', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'ObjectCrName' },
        { name: 'FinanceSourceName' },
        { name: 'ContragentName' },
        { name: 'TypeContractObject' },
        { name: 'DocumentName' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'DateFrom', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'SumContract', defaultValue: null },
        { name: 'StartSumContract', defaultValue: null },
        { name: 'BudgetMo', defaultValue: null },
        { name: 'BudgetSubject', defaultValue: null },
        { name: 'OwnerMeans', defaultValue: null },
        { name: 'FundMeans', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'TypeWork', defaultValue: null },
        { name: 'DateStartWork', defaultValue: null },
        { name: 'DateEndWork', defaultValue: null },
        { name: 'UsedInExport', defaultValue: 20 },
        { name: 'Customer' }
    ]
});