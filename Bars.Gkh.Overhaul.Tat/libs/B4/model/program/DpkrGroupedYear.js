Ext.define('B4.model.program.DpkrGroupedYear', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrGroupedYear'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'Year' },
        { name: 'Sum' }
    ]
});