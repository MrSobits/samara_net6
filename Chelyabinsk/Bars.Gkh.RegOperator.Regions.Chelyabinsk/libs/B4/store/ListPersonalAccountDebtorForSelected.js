Ext.define('B4.store.ListPersonalAccountDebtorForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.regop.personal_account.BasePersonalAccount'],
    autoLoad: false,
    model: 'B4.model.regop.personal_account.BasePersonalAccount'    
});