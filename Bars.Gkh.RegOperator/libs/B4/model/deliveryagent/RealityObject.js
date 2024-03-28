Ext.define('B4.model.deliveryagent.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DeliveryAgentRealObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DeliveryAgent', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Name' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'ContragentState' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});