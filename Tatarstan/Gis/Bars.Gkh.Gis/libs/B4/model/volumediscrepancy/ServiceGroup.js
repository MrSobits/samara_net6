Ext.define('B4.model.volumediscrepancy.ServiceGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseServiceRegister',
        listAction: 'ServiceGroupList'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});