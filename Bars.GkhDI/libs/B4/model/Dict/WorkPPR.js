Ext.define('B4.model.dict.WorkPpr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkPpr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'GroupWorkPpr', defaultValue: null },
        { name: 'GroupWorkPprName' },
        { name: 'Measure' }
    ]
});