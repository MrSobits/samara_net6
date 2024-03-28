Ext.define('B4.model.version.VersionRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.ChangeBasisType'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'VersionRecord',
        timeout: 2 * 60 * 1000 // 2 минуты
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'CommonEstateObjects' },
        { name: 'Year' },
        { name: 'FixedYear' },
        { name: 'YearCalculated' },
        { name: 'ChangeBasisType' },
        { name: 'IndexNumber' },
        { name: 'IsChangedYear' },
        { name: 'HouseNumber' },
        { name: 'Point' },
        { name: 'Sum' },
        { name: 'Changes' },
<<<<<<< HEAD
        { name: 'Remark' },
        { name: 'StructuralElements' },
        { name: 'EntranceNum' },
        { name: 'KPKR' },
        { name: 'Hidden' },
        { name: 'IsSubProgram' }
=======
        { name: 'StructuralElements' },
        { name: 'WorkCode' }
>>>>>>> net6
    ]
});