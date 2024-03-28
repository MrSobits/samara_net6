Ext.define('B4.model.dict.TypeService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'typeservice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});