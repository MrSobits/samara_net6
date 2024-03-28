Ext.define('B4.model.dict.Work', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeWork'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'OvrhlWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'ReformCode' },
        { name: 'GisGkhCode' },
        { name: 'GisGkhGuid' },
        { name: 'GisCode' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'UnitMeasureName' },
        { name: 'Normative' },
        { name: 'Description' },
        { name: 'Consistent185Fz', defaultValue: false },
        { name: 'IsAdditionalWork', defaultValue: false },
        { name: 'IsConstructionWork', defaultValue: false },
        { name: 'IsPSD', defaultValue: false },
        { name: 'TypeWork', defaultValue: 10 },
        { name: 'FinSources' },
        { name: 'IsActual', defaultValue: true },
        { name: 'WithinShortProgram', defaultValue: false }
    ]
});