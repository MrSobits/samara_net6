Ext.define('B4.model.inspectiongji.AppealCits', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionAppealCits'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Inspection', defaultValue: null },
        { name: 'AppealCits', defaultValue: null }
    ]
});