Ext.define('B4.model.tatarstanprotocolgji.TatarstanProtocolGjiRealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanProtocolGjiRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TatarstanProtocolGji' },
        { name: 'RealityObject' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Area' }
    ]
});