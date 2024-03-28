Ext.define('B4.model.claimwork.BuilderViolatorViol', {
	extend: 'B4.base.Model',

	fields: [
	    { name: 'Id' },
	    { name: 'BuilderViolator' },
		{ name: 'Violation' },
		{ name: 'Note' }
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'BuilderViolatorViol'
	}
});
