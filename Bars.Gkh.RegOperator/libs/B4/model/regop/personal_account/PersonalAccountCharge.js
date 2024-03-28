Ext.define('B4.model.regop.personal_account.PersonalAccountCharge', {
    extend: 'B4.base.Model',

    fields: [
		{ name: 'Id' },
		{ name: 'ChargeDate' },
		{ name: 'AccountState' },
		{ name: 'Guid' },
		{ name: 'PeriodId' },
		{ name: 'Charge' },
		{ name: 'ChargeTariff' },
		{ name: 'ChargeBaseTariff' },
		{ name: 'OverPlus' },
		{ name: 'Penalty' },
		{ name: 'RecalcPenalty' },
		{ name: 'RecalcByBaseTariff' },
		{ name: 'RecalcByDecisionTariff' },
		{ name: 'AccountId' },
		{ name: 'AccountNum' },
		{ name: 'AccountFormationVariant' },
		{ name: 'ContragentAccountNumber' },
		{ name: 'Description' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountCharge'
    }
});
