Ext.define('B4.model.dict.TypeOwnership', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'typeownership'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});