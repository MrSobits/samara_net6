Ext.define('B4.model.metavalueconstructor.BaseDataValue', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseDataValue'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Value' },
        { name: 'ObjectType' }
    ]
});