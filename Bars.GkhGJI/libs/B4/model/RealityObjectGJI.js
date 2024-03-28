Ext.define('B4.model.RealityObjectGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionGjiRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionGji', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'MunicipalityName' },
        { name: 'ActCheckName' },
        { name: 'Address' },
        { name: 'Area' },
        { name: 'RoomNums' }
    ]
});