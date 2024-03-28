Ext.define('B4.model.House', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'House'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' }
    ]
});