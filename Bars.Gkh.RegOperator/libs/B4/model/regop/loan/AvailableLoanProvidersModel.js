Ext.define('B4.model.regop.loan.AvailableLoanProvidersModel', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Address' },
        { name: 'RealityObject' },
        { name: 'PlanYear' },
        { name: 'AvailableMoney' },
        { name: 'LoanSum' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLoan',
        listAction: 'ListLoanProviders'
    }
});