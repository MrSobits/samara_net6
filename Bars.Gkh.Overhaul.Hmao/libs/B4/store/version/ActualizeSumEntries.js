Ext.define('B4.store.version.ActualizeSumEntries', {
    extend: 'B4.base.Store',
    fields: ['Id', 'RealityObject', 'CommonEstateObjects', 'Year', 'IndexNumber', 'Sum'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetActualizeSumEntriesList',
        timeout: 30 * 60 * 1000 // 30 минут
    }
});