Ext.define('B4.model.ShortProgramRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address', defaultValue: null },
        { name: 'PlanYear' },
        { name: 'CeoName' },
        { name: 'Sum' },
        { name: 'BudgetOwners' },
        { name: 'BudgetRegion' },
        { name: 'BudgetMunicipality' },
        { name: 'BudgetFcr' },
        { name: 'AdditionalExpences' },
        { name: 'BudgetOtherSource' }
    ]
});