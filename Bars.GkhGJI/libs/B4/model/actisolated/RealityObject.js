Ext.define('B4.model.actisolated.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedRealObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolated', defaultValue: null },
        { name: 'Municipality' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'HaveViolation', defaultValue: 30 },
        { name: 'ViolationCount', defaultValue: 0 },
        { name: 'Description' }
    ]
});