Ext.define('B4.model.volumediscrepancy.Settlement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseServiceRegister',
        listAction: 'SettlementList'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});