Ext.define('B4.model.claimwork.BuildContractClwViol', {
	extend: 'B4.base.Model',

	fields: [
	    { name: 'Id' },
	    { name: 'ClaimWork' },
		{ name: 'Violation' },
		{ name: 'Note' }
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'BuildContractClwViol'
	}
});
