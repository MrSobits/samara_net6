Ext.define('B4.model.version.VersionRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VersionRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'CommonEstateObjects' },
        { name: 'Year' },
        { name: 'IndexNumber' },
        { name: 'Point' },
        { name: 'Sum' },
        { name: 'IsChangedYear' },
        { name: 'File' },
        { name: 'DocumentName' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'PeriodStart' },
        { name: 'PeriodEnd' }, 
        { name: 'Changes' },
        { name: 'Remark' },
        { name: 'IsSubProgram' }
    ]
});