Ext.define('B4.model.dynamic.AttributeValue', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PassportAttributeValue'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MetaId' },
        { name: 'Name' }
    ]
});