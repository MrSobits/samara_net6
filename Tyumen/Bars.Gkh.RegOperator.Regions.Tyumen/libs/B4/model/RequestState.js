Ext.define('B4.model.RequestState', {
    extend: 'B4.base.Model',
	idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
		controllerName: 'RequestState'
	},
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Description' },
        { name: 'File' },
        { name: 'RealityObject' },
        { name: 'UserName'},
        { name: 'ObjectCreateDate' }
    ]
});