Ext.define('B4.model.administration.PrintCertHistory', {
    extend: 'B4.base.Model',
	idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
		controllerName: 'PrintCertHistory'
	},
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AccNum' },
        { name: 'Address' },
        { name: 'Type' },
		{ name: 'Name' },
		{ name: 'PrintDate' },
		{ name: 'Username' },
		{ name: 'Role' }
    ]
});