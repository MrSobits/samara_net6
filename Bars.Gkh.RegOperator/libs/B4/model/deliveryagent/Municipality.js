Ext.define('B4.model.deliveryagent.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DeliveryAgentMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DeliveryAgent', defaultValue: null },
        { name: 'Municipality', defaultValue: null }
    ]
});