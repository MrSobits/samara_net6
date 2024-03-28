Ext.define('B4.model.regop.personal_account.RoomAccounts', {
    extend: 'B4.base.Model',

    fields: [
		{ name: 'PersonalAccountNum' },
		{ name: 'AreaShare' },
		{ name: 'NewShare' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        listAction: 'RoomAccounts'
    }
});
