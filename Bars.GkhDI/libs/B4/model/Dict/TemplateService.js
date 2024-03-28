Ext.define('B4.model.dict.TemplateService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TemplateService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Characteristic' },
        { name: 'Changeable', defaultValue: false },
        { name: 'IsMandatory', defaultValue: false },
        { name: 'TypeGroupServiceDi', defaultValue: 10 },
        { name: 'KindServiceDi', defaultValue: 10 },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'CommunalResourceType', defaultValue: null },
        { name: 'HousingResourceType', defaultValue: null },
        { name: 'UnitMeasureName' }, 
        { name: 'IsConsiderInCalc', defaultValue: false }, 
        { name: 'ActualYearStart' }, 
        { name: 'ActualYearEnd' }
    ]
});