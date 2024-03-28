Ext.define('B4.store.regop.Loan', {
    extend: 'B4.base.Store',
    fields: [
        { name: 'Id' },
        { name: 'State' },
        { name: 'LoanDate' },
        { name: 'LoanSource' },
        { name: 'ProgramCr' },
        { name: 'LoanReceiver' },
        { name: 'Settlement' },
        { name: 'Municipality' },
        { name: 'LoanSum' },
        { name: 'LoanReturnedSum' },
        { name: 'DebtSum' },
        { name: 'Saldo' },
        { name: 'FactEndDate' },
        { name: 'Sources' }
    ],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLoan',
        listAction: 'List'
    }
});