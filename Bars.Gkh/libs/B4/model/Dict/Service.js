Ext.define('B4.model.dict.Service', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceDictionary',
        timeout: 9999999
    },
    fields: [
        { name: 'Id' },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Measure' },
        { name: 'TypeService' },
        { name: 'TypeCommunalResource' },
        { name: 'IsProvidedForAllHouseNeeds'},
        { name: 'UnitMeasure'}
    ]
});