Ext.define('B4.model.documents.RealityObjProtocol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentsRealityObjProtocol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'FileName' },
        { name: 'Year' },
        { name: 'DocNum' },
        { name: 'DocDate' },
        { name: 'DisclosureInfoRealityObj'}
    ]
});