Ext.define('B4.model.LogEntityProperty', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LogEntityProperty'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PropertyName' },
        { name: 'OldValue' },
        { name: 'NewValue' },
        { name: 'Type' }
    ]
});
