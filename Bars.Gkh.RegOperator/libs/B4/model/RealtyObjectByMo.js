Ext.define('B4.model.RealtyObjectByMo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObject',
        listAction: 'ListByMoSettlement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});