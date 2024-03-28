Ext.define('B4.model.objectcr.estimate.EstimateCalculation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EstimateCalculation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr', defaultValue: null },
        { name: 'TypeWork', defaultValue: null },
        { name: 'ObjectCrName' },
        { name: 'ResourceStatmentDocumentName' },
        { name: 'EstimateDocumentName' },
        { name: 'FileEstimateDocumentName' },
        { name: 'ResourceStatmentDocumentNum' },
        { name: 'EstimateDocumentNum' },
        { name: 'FileEstimateDocumentNum' },
        { name: 'ResourceStatmentDateFrom', defaultValue: null },
        { name: 'EstimateDateFrom', defaultValue: null },
        { name: 'FileEstimateDateFrom', defaultValue: null },
        { name: 'TypeWorkCr', defaultValue: null },
        { name: 'TypeWorkCrName' },
        { name: 'FinanceSourceName' },
        { name: 'OtherCost', defaultValue: null },
        { name: 'TotalEstimate', defaultValue: null },
        { name: 'TotalDirectCost', defaultValue: null },
        { name: 'OverheadSum', defaultValue: null },
        { name: 'Nds', defaultValue: null },
        { name: 'EstimateProfit', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'ResourceStatmentFile', defaultValue: null },
        { name: 'EstimateFile', defaultValue: null },
        { name: 'FileEstimateFile', defaultValue: null },
        { name: 'ObjectCrId' },
        { name: 'Address' },
        { name: 'IsSumWithoutNds', defaultValue: false },
        { name: 'TotalEstimateSum' }, //сумма по полю общая стоимость в позициях сметы (вкладка сметы)
        { name: 'TotalResourceSum' }, //сумма по полю общая стоимость (вкладка ведомость ресурсов)
        { name: 'EstimationType', defaultValue: 0 },
        { name: 'UsedInExport', defaultValue: 20 }
    ]
});