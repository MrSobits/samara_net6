Ext.define('B4.model.dict.PublicService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});