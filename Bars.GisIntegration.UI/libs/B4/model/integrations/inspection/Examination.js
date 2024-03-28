Ext.define('B4.model.integrations.inspection.Examination', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionService',
        listAction: 'GetInspectionList',
        timeout: 5 * 60 * 1000
    },
    fields: [
        { name: 'Id' },
        { name: 'Number' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'KindCheck' },
        { name: 'ContragentName' },
        { name: 'Base' }
    ]
});
