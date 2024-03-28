Ext.define('B4.store.regop.personal_account.PersonalAccountFieldDetails', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Amount', 'Period'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonAccountDetalization',
        listAction: 'GetFieldDetails'
    }
});