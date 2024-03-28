Ext.define('B4.model.realityobj.Image', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.ImagesGroup'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectImage'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'ToPrint', defaultValue: false },
        { name: 'DateImage' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'ImagesGroup', defaultValue: 10 },
        { name: 'Period', defaultValue: null },
        { name: 'PeriodName', defaultValue: null },
        { name: 'WorkCr', defaultValue: null },
        { name: 'WorkCrName', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});