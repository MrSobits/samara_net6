Ext.define('B4.store.version.ActualizeAddEntries', {
    extend: 'B4.base.Store',
    fields: ['Id', 'RealityObject', 'CommonEstateObject', 'StructuralElement'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetAddEntriesList',
        timeout: 30 * 60 * 1000 // 30 минут
    }
});