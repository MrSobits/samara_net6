Ext.define('B4.model.appealcits.AppealCitsInfo', {
	extend: 'B4.base.Model',
	idProperty: 'Id',
	proxy: {
		type: 'b4proxy',
		controllerName: 'AppealCitsInfo'
	},
	fields: [
		{ name: 'Id', useNull: true },
		{ name: 'DocumentNumber' },
		{ name: 'AppealDate' },
		{ name: 'OperationDate' },
		{ name: 'Correspondent' },
		{ name: 'OperationType' },
		{ name: 'Operator' }
	]
});