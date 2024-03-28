Ext.define('B4.model.integrations.inspection.Plan', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionService',
        listAction: 'GetPlanList'
    },
    fields: [
        { name: 'Id'},
        { name: 'Name' },
        { name: 'DateApproval' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});
