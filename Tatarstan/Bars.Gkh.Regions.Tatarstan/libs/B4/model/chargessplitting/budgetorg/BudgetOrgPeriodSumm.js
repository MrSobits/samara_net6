Ext.define('B4.model.chargessplitting.budgetorg.BudgetOrgPeriodSumm', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BudgetOrgContractPeriodSumm',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'Organization' },
        { name: 'TypeCustomer' },
        { name: 'PublicServiceOrg' },
        { name: 'Service' },
        { name: 'Charged' },
        { name: 'Paid' },
        { name: 'EndDebt' }
    ]
});