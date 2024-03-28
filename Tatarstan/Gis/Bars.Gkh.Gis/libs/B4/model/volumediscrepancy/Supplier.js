Ext.define('B4.model.volumediscrepancy.Supplier', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseServiceRegister',
        listAction: 'SupplierList'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});