Ext.define('B4.model.protocolrso.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolRSORealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolRSO', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Area' }
    ]
});