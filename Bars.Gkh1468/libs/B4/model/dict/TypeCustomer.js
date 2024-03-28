Ext.define('B4.model.dict.TypeCustomer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeCustomer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});