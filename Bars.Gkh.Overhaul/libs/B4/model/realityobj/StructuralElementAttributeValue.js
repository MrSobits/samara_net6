Ext.define('B4.model.realityobj.StructuralElementAttributeValue', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElementAttributeValue'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Object', useNull: true },
        { name: 'Attribute', useNull: true },
        { name: 'Value' }
    ]
});