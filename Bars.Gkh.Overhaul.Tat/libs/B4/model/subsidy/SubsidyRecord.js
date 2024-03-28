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
        { name: 'SubsidyMunicipality' },
        { name: 'OwnerSource' },
        { name: 'BudgetFcr' },
        { name: 'BudgetCr' },
        { name: 'NeedFinance' },
        { name: 'Deficit' }
    ]
});