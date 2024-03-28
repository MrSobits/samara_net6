Ext.define('B4.model.inspectiongji.Risk', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionRisk'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Inspection', defaultValue: null },
        { name: 'RiskCategory', defaultValue: null },
        { name: 'StartDate' },
        { name: 'EndDate' }
    ]
});