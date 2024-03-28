Ext.define('B4.model.activitytsj.ProtocolRealObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityTsjProtocolRealObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'ActivityTsjProtocol', defaultValue: null },
        { name: 'Address' },
        { name: 'Municipality' }
    ]
});