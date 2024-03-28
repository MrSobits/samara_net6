Ext.define('B4.model.appealcits.BaseStatement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseStatement',
        listAction: 'ListByAppealCits'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionNumber' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'RealtyObject' },
		{ name: 'TypeBase' },
		{ name: 'DocumentGjiInspector'}
    ]
});