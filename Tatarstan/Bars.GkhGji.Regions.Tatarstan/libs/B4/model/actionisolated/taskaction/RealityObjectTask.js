Ext.define('B4.model.actionisolated.taskaction.RealityObjectTask', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskActionIsolatedRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionGji', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Area' }
    ]
});