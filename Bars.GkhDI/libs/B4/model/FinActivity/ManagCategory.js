Ext.define('B4.model.finactivity.ManagCategory', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityManagCategory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'TypeCategoryHouseDi', defaultValue: 10 },
        { name: 'IncomeManaging', defaultValue: null },
        { name: 'IncomeUsingGeneralProperty', defaultValue: null },
        { name: 'ExpenseManaging', defaultValue: null },
        { name: 'ExactPopulation', defaultValue: null },
        { name: 'DebtPopulationStart', defaultValue: null },
        { name: 'DebtPopulationEnd', defaultValue: null },
        { name: 'IsInvalid', defaultValue: false }
    ]
});
