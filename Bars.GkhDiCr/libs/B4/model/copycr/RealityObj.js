Ext.define('B4.model.copycr.RealityObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjCopyCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AddressName' },
        { name: 'AreaLiving' },
        { name: 'DateLastOverhaul' },
        { name: 'RealityObjectId' }
    ]
});