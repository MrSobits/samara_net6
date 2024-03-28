Ext.define('B4.model.workscr.Inspection', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorksCrInspection'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeWork', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'Official', defaultValue: null },
        { name: 'DocumentNumber', defaultValue: null },
        { name: 'PlanDate', defaultValue: null },
        { name: 'FactDate', defaultValue: null },
        { name: 'InspectionState', defaultValue: 0 },
        { name: 'Reason', defaultValue: null },
        { name: 'Description', defaultValue: null }
    ]
});