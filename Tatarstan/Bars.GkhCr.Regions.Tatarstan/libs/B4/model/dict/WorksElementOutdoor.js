Ext.define('B4.model.dict.WorksElementOutdoor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorksElementOutdoor'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'TypeWork' },
        { name: 'UnitMeasure' }
    ]
});