Ext.define('B4.model.regop.personal_account.PersonalAccountsByRo', {
    extend: 'B4.base.Model',

    fields: [
		{ name: 'AccountNum' },
		{ name: 'AccountOwner' },
		{ name: 'RoomNum' },
		{ name: 'Id' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        listAction: 'PersonalAccountsByRo'
    }
});
