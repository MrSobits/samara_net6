Ext.define('B4.model.claimwork.lawsuit.Court', {
	extend: 'B4.base.Model',

	fields: [
		{ name: 'Id' },
		{ name: 'DocumentClw' },
		{ name: 'LawsuitCourtType', defaultValue:0 },
		{ name: 'DocDate' },
		{ name: 'DocNumber' },
		{ name: 'File' },
		{ name: 'Description' },
		{ name: 'PretensionType' },
		{ name: 'PretensionReciever' },
		{ name: 'PretensionDate' },
		{ name: 'PretensionResult' },
		{ name: 'PretensionReviewDate' },
		{ name: 'PretensionNote' }
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'LawsuitClwCourt'
	}
});
