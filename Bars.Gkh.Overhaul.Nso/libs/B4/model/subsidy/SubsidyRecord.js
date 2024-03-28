Ext.define('B4.model.subsidy.SubsidyRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SubsidyRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SubsidyYear' },
        { name: 'BudgetRegion' },
        { name: 'BudgetMunicipality' },
        { name: 'BudgetFcr' },
        { name: 'BudgetOtherSource' },
        { name: 'PlanOwnerCollection' },
        { name: 'PlanOwnerPercent' },
        { name: 'NotReduceSizePercent' },
        { name: 'OwnerSumForCr' },
        { name: 'BudgetCr' },
        { name: 'CorrectionFinance' },
        { name: 'AdditionalExpences' },
        { name: 'IsShortTerm' },
        { name: 'BalanceAfterCr' },
        { name: 'IsSummary' }
    ]
});