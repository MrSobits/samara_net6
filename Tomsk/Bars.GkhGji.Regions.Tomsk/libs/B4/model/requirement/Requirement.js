Ext.define('B4.model.requirement.Requirement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'Requirement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Document', defaultValue: null },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'ArticleLaw' },
        { name: 'Destination' },
        { name: 'TypeRequirement' },
        { name: 'MaterialSubmitDate' },
        { name: 'InspectionDate' },
        { name: 'InspectionHour' },
        { name: 'InspectionMinute' },
        { name: 'AddMaterials' },
        { name: 'State', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});