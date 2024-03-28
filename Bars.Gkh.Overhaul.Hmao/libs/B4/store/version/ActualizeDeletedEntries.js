Ext.define('B4.store.version.ActualizeDeletedEntries', {
    extend: 'B4.base.Store',
    fields: ['Id', 'RealityObject', 'CommonEstateObjects', 'Year', 'IndexNumber', 'Sum', 'IsChangedYear'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetDeletedEntriesList',
        timeout: 30 * 60 * 1000 // 30 минут
    }
});