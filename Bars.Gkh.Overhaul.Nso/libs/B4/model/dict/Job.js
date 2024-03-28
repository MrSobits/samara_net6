Ext.define('B4.model.dict.Job', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Job'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'Work', defaultValue: null }
    ]
});