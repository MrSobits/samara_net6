Ext.define('B4.model.regop.loan.LoanProvider', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'LoanDate' },
        { name: 'ProgramCr' },
        { name: 'Address' },
        { name: 'LoanSum' },
        { name: 'LoanDebt' },
        { name: 'DebtSum' },
        { name: 'LoanReturnedSum' },
        { name: 'PlanLoanMonthCount' },
        { name: 'FactEndDate' },
        { name: 'Document' },
        { name: 'LoanProvider' },
        { name: 'LoanSource' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLoan',
        extraParams: {
            dir: 'OutgoingLoan'
        }
    }
});