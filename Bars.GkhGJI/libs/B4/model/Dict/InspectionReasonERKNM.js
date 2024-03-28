Ext.define('B4.model.dict.InspectionReasonERKNM', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionReasonERKNM'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'ERKNMDocumentType', defaultValue: 0 },
        { name: 'ERKNMId' },
    ]
});