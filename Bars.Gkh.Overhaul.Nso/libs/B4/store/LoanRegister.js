Ext.define('B4.store.LoanRegister', {
    extend: 'B4.base.Store',
    requires: ['B4.model.longtermprobject.Loan'],
    autoLoad: false,
    model: 'B4.model.longtermprobject.Loan',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LongTermObjectLoan',
        listAction: 'ListRegister'
    }
});