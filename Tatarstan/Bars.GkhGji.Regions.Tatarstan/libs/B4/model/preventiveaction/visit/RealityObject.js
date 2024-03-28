Ext.define('B4.model.preventiveaction.visit.RealityObject', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheet',
        listAction: 'GetViolationRealityObjectsList'
    },
    fields: [
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'RealityObjectId' },
    ]
});