Ext.define('B4.model.deliveryagent.DeliveryAgent', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Name' },
        { name: 'ContragentState' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'DeliveryAgent'
    }
});