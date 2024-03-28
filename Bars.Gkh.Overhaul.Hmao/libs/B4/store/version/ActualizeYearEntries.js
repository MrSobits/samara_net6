Ext.define('B4.store.version.ActualizeYearEntries', {
    extend: 'B4.base.Store',
    fields: [
        'Id',
        'RealityObject',
        'CommonEstateObject',
        'Year',
        'Sum',
        'Changes',
        'IsChangedYear'
    ],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetActualizeYearEntriesList',
        timeout: 30 * 60 * 1000 // 30 минут
    }
});