Ext.define('B4.model.RequestStatePerson', {
    extend: 'B4.base.Model',
	idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
		controllerName: 'RequestStatePerson'
	},
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Email' },
        { name: 'Position' },
		{ name: 'Description', defaultValue: null },
		{ name: 'Status' }
    ]
});