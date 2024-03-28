Ext.define('B4.model.protocolmhc.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolMhcRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolMhc', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Area' }
    ]
});