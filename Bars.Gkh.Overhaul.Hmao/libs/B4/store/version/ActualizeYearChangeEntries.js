Ext.define('B4.store.version.ActualizeYearChangeEntries', {
    extend: 'B4.base.Store',
    fields: [
        'Id',
        'RealityObject',
        'CommonEstateObject',
        'Sum',
        'Year',
        'Changes',
        'IsChangedYear'
    ],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetActualizeYearChangeEntriesList',
        timeout: 30 * 60 * 1000 // 30 минут
    }
});