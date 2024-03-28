Ext.define('B4.model.version.ChangeYearOwnerDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChangeYearOwnerDecision',
        timeout: 2 * 60 * 1000 // 2 минуты
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'IndexNumber' },
        { name: 'RealityObject' },
        { name: 'CommonEstateObject' },
        { name: 'StructuralElement' },
        { name: 'OldYear' },
        { name: 'NewYear' },
        { name: 'File' }
    ]
});