
Ext.define('PersonalAccount', {
	extend: 'B4.base.Model',

	fields: [
		{name: 'Room' },
		{name: 'AccountOwner' },
		{name: 'PersonalAccountNum' },
		{name: 'AreaShare' },
		{name: 'ChargedSum' },
		{name: 'PaidSum' },
		{name: 'PenaltySum' },
		{name: 'Id' }
	],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccount'
    }
});
