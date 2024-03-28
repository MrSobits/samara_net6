Ext.define('B4.model.realityobj.Document', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'CreateDate' },
        { name: 'Name' },
        { name: 'DocumentType', defaultValue: 10 },
        { name: 'File' }
    ]
});