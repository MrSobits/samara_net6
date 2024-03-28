Ext.define('B4.model.claimwork.Pretension', {
	extend: 'B4.base.Model',

	fields: [
		{ name: 'Id' },
		{ name: 'ClaimWork' },
		{ name: 'ClaimWorkTypeBase' },
		{ name: 'DocumentType' },
		{ name: 'DocumentDate' },
		{ name: 'DocumentNumber' },
		{ name: 'State' },
		{ name: 'DateReview' },
		{ name: 'DebtBaseTariffSum' },
		{ name: 'DebtDecisionTariffSum' },
		{ name: 'Sum' },
		{ name: 'Penalty' },
		{ name: 'SumPenaltyCalc' },
		{ name: 'File' },
		{ name: 'SendDate' },
		{ name: 'RequirementSatisfaction' },
		{ name: 'BaseInfo' },
		{ name: 'Municipality' },
		{ name: 'Address' },
        { name: 'PaymentPlannedPeriod', type: 'date' },
        { name: 'NumberPretension'}
		
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'PretensionClw'
	}
});
