Ext.define('B4.model.documentsgjiregister.MotivatedPresentation', {
	extend: 'B4.model.DocumentGji',
	idProperty: 'Id',
	proxy: {
		type: 'b4proxy',
		controllerName: 'MotivatedPresentation',
		listAction: 'ListForRegistry'
	},
	fields: [
		{ name: 'State', defaultValue: null },
		{ name: 'MunicipalityNames' },
		{ name: 'InspectionId' },
		{ name: 'TypeBase' },
		{ name: 'InspectorNames' },
		{ name: 'RoAddresses' },
		{ name: 'DocumentNumber' },
		{ name: 'DocumentDate' },
		{ name: 'RealityObjectCount' },
		{ name: 'TypeDocumentGji' }
	]
});