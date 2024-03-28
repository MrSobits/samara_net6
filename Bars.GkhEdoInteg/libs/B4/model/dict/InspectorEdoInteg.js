Ext.define('B4.model.dict.InspectorEdoInteg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectorCompareEdo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CompareId', useNull: true },
        { name: 'Fio' },
        { name: 'Code' },
        { name: 'CodeEdo' },
        { name: 'Inspector', defaultValue: null }
    ]
});