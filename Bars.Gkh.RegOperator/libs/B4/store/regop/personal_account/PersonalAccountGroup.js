Ext.define('B4.store.regop.personal_account.PersonalAccountGroup', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Id', 'Name', 'IsSystem'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonAccountGroup',
        listAction: 'ListGroups',
        timeout: 60000
    }
});

