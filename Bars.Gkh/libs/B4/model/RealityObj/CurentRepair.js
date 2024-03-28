Ext.define('B4.model.realityobj.CurentRepair', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectCurentRepair'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'WorkKind', defaultValue: null },
        { name: 'UnitMeasure' },
        { name: 'WorkKindName' },
        { name: 'PlanDate' },
        { name: 'PlanSum' },
        { name: 'PlanWork' },
        { name: 'FactDate' },
        { name: 'FactSum' },
        { name: 'FactWork' },
        { name: 'Builder' }
    ]
});