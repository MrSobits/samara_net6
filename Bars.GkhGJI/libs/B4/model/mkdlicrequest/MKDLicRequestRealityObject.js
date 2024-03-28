Ext.define('B4.model.mkdlicrequest.MKDLicRequestRealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestRealityObject'
    },
    fields: [
        { name: 'Id' },
        { name: 'RealityObject' },
        { name: 'Municipality' },
        { name: 'MKDLicRequest'}       
    ]
});