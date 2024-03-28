Ext.define('B4.model.manualintegration.RefRealityObjectSelected',
{
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: { type: 'memory' },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },
        { name: 'ExternalId' }
    ]
});