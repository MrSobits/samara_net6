Ext.define('B4.store.regop.personal_account.PersonalAccountOperation', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Id', 'AccountId', 'Date', 'Charged', 'Recalc', 'Paid', 'ChargedPenalty', 'PaidPenalty'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        listAction: 'ListOperations',
        timeout: 60000
    }
});

