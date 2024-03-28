Ext.define('B4.model.realityobj.Protocol', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectProtocol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'DocumentName' },
        { name: 'DocumentNum' },
        { name: 'File', defaultValue: null},
        { name: 'DateFrom' },
        { name: 'CouncilResult', defaultValue: 10 }
    ]
});