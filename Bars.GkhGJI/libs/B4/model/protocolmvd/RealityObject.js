Ext.define('B4.model.protocolmvd.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolMvdRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolMvd', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Area' }
    ]
});