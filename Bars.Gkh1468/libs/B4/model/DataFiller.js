Ext.define('B4.model.DataFiller', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DataFiller'
    },
    idProperty: 'Code',
    fields: [
        { name: 'Code' },
        { name: 'Name' }
    ]
});