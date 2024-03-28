Ext.define('B4.model.groups.RealityObjGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjGroup'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'AddressName', defaultValue: null }
    ]
});