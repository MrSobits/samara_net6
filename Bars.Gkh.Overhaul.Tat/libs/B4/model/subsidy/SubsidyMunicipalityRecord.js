Ext.define('B4.model.subsidy.SubsidyMunicipalityRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SubsidyMunicipalityRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SubsidyYear' },
        { name: 'BudgetRegion' },
        { name: 'BudgetMunicipality' },
        { name: 'BudgetFcr' },
        { name: 'SubsidyMunicipality' },
        { name: 'OwnerSource' },
        { name: 'BudgetFcr' },
        { name: 'BudgetCr' },
        { name: 'NeedFinance' },
        { name: 'Deficit' },
        { name: 'CorrNeedFinance' },
        { name: 'CorrDeficit' },
        { name: 'IsShortTerm' },
        { name: 'IsSummary' }
    ]
});