Ext.define('B4.model.claimwork.lawsuit.Doc', {
	extend: 'B4.base.Model',

	fields: [
		{ name: 'Id' },
		{ name: 'DocumentClw' },
		{ name: 'DocName' },
		{ name: 'DocDate' },
		{ name: 'DocNumber' },
		{ name: 'File' },
		{ name: 'Description' }
	],

	proxy: {
		type: 'b4proxy',
		controllerName: 'LawsuitClwDocument'
	}
});
