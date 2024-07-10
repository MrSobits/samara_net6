Ext.define('B4.model.PrintFormCategory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrintFormCategory'
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'Name' }
    ]
});