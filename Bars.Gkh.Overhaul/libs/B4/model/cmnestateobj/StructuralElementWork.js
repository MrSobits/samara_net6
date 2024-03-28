Ext.define('B4.model.cmnestateobj.StructuralElementWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StructuralElementWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Job', defaultValue: null },
        { name: 'StructuralElement', defaultValue: null },
        { name: 'WorkName' },
        { name: 'UnitMeasure' }
    ]
});