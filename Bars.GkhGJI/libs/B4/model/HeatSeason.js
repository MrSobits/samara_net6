Ext.define('B4.model.HeatSeason', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatSeason'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Period', defaultValue: null },
        { name: 'DateHeat' },
        { name: 'HeatingSystem', defaultValue: 10 },
        { name: 'Municipality' },
        { name: 'AreaMkd' },
        { name: 'DateHeat' },
        { name: 'ActFlushing' },
        { name: 'ActPressing' },
        { name: 'ActVentilation' },
        { name: 'ActChimney' },
        { name: 'Passport' },
        { name: 'ManOrgName' },
        { name: 'TypeHouse' },
        { name: 'Floors' },
        { name: 'MaximumFloors' },
        { name: 'NumberEntrances' },
        { name: 'Address' },
        { name: 'HeatingSeasonId', useNull: true }
    ]
});