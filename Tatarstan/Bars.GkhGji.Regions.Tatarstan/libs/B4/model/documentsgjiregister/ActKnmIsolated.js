Ext.define('B4.model.documentsgjiregister.ActKnmIsolated', {
	extend: 'B4.model.DocumentGji',
	idProperty: 'Id',
	proxy: {
		type: 'b4proxy',
		controllerName: 'ActActionIsolated',
		listAction: 'ListForRegistry'
	},
	fields: [
		{ name: 'State', defaultValue: null },
		{ name: 'MunicipalityNames' },
		{ name: 'TypeBaseAction' },
		{ name: 'InspectorNames' },
		{ name: 'RoAddresses' },
		{ name: 'DocumentNumber' },
		{ name: 'DocumentNum' },
		{ name: 'DocumentDate' },
		{ name: 'ContragentName' },
		{ name: 'PersonName' },
		{ name: 'RealityObjectCount' },
		{ name: 'HaveViolation' },
		{ name: 'DocumentCount' },
		{ name: 'InspectionId' },
		{ name: 'TypeBase' },
		{ name: 'TypeDocumentGji'}
	]
});