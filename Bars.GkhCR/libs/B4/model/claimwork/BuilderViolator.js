Ext.define('B4.model.claimwork.BuilderViolator', {
	extend: 'B4.base.Model',

	fields: [
	    { name: 'Id' },
	    { name: 'BuildContract' },
		{ name: 'Municipality' },
		{ name: 'Settlement' },
		{ name: 'Builder' },
		{ name: 'Inn' },
		{ name: 'DocumentNum' },
		{ name: 'DocumentDateFrom' },
		{ name: 'DateEndWork' },
	    { name: 'CountDaysDelay' },
	    { name: 'CreationType', defaultValue: 20 },
	    { name: 'StartingDate' },
	    { name: 'ObjCrId' },
	    { name: 'Address' },
        { name: 'IsClaimWorking' }
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'BuilderViolator'
	}
});
