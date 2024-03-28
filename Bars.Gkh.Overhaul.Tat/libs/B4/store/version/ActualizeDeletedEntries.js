Ext.define('B4.store.version.ActualizeDeletedEntries', {
    extend: 'B4.base.Store',
    fields: ['Id', 'RealityObject', 'CommonEstateObjects', 'Year', 'IndexNumber', 'Sum', 'CorrectYear'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetDeletedEntriesList'
    }
});