Ext.define('B4.model.specialobjectcr.FinanceSourceRes', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialFinanceSourceResource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'FinanceSource' },
        { name: 'FinanceSourceName' },
        { name: 'BudgetMu' },
        { name: 'BudgetSubject' },
        { name: 'OwnerResource' },
        { name: 'FundResource' },
        { name: 'BudgetMuIncome' },
        { name: 'BudgetSubjectIncome' },
        { name: 'FundResourceIncome' },
        { name: 'BudgetMuPercent' },
        { name: 'BudgetSubjectPercent' },
        { name: 'FundResourcePercent' },
        { name: 'TypeWorkCr' },
        { name: 'Year' },
        { name: 'OtherResource' },
        { name: 'GroupField' }
    ]
});