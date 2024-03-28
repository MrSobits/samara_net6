Ext.define('B4.model.regop.personal_account.PersonalAccountBenefits', {
    extend: 'B4.base.Model',

    fields: [
		{ name: 'PersonalAccount' },
		{ name: 'Period' },
		{ name: 'Sum' },
		{ name: 'Id' },
		{ name: 'PersAccNum' },
		{ name: 'Owner' },
		{ name: 'Address' },
		{ name: 'BenefitsName' },
		{ name: 'BenefitsDateStart' },
		{ name: 'BenefitsDateEnd' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountBenefits'
    }
});
