Ext.define('B4.model.actisolated.Measure', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedRealObjMeasure'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolatedRealObj', defaultValue: null },
        { name: 'Measure' }
    ]
});