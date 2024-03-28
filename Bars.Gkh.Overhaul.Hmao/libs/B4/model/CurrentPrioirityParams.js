Ext.define('B4.model.CurrentPrioirityParams', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CurrentPrioirityParams'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Order' },
        { name: 'Name' }
    ]
});