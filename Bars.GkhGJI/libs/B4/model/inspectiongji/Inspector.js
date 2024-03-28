Ext.define('B4.model.inspectiongji.Inspector', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionGjiInspector'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Inspection', defaultValue: null },
        { name: 'Inspector', defaultValue: null }
    ]
});