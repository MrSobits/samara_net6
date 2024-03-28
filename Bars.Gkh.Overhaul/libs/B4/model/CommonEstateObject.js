Ext.define('B4.model.CommonEstateObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CommonEstateObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'ReformCode' },
        { name: 'GisCode' },
        { name: 'GroupType', defaultValue: null },
        { name: 'Name' },
        { name: 'ShortName' },
        { name: 'IsMatchHc' },
        { name: 'IncludedInSubjectProgramm' },
        { name: 'IsEngineeringNetwork' },
        { name: 'MultipleObject' },
        { name: 'Weight' },
        { name: 'IsMain' }
    ]
});