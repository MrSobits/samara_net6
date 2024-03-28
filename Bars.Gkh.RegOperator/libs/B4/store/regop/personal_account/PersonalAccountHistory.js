Ext.define('B4.store.regop.personal_account.PersonalAccountHistory', {
    extend: 'B4.base.Store',
    requires: ['B4.model.PersonalAccountChange'],
    model: 'B4.model.PersonalAccountChange',
    autoLoad: false
});