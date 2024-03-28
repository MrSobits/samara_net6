Ext.define('B4.model.volumediscrepancy.Street', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseServiceRegister',
        listAction: 'StreetList'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});