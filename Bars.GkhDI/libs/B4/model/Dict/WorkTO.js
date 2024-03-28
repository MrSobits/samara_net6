Ext.define('B4.model.dict.WorkTo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkTo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'GroupWorkTo', defaultValue: null },
        { name: 'GroupWorkToName' }
    ]
});