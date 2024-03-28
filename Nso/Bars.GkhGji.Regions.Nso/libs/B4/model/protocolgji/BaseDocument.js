Ext.define('B4.model.protocolgji.BaseDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'KindBaseDocument' },
        { name: 'DateDoc' },
        { name: 'NumDoc' },
        { name: 'RealityObject', mapping: 'RealityObject.Address' },
        { name: 'Protocol' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolBaseDocument'
    },
});