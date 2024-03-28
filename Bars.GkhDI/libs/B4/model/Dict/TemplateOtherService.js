Ext.define('B4.model.dict.TemplateOtherService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TemplateOtherService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Characteristic' },
        { name: 'UnitMeasure' },
        { name: 'UnitMeasureName' }
    ]
});