Ext.define('B4.model.manualintegration.RefRealityObject',
{
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManualIntegration',
        listAction: 'ListManagedRealityObjects'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },
        { name: 'ExternalId' }
    ]
});