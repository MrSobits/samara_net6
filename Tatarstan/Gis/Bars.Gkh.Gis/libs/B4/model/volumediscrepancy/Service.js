Ext.define('B4.model.volumediscrepancy.Service', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseServiceRegister',
        listAction: 'ServiceList'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});