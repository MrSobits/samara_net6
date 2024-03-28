Ext.define('B4.store.overhaulpropose.CrObjectWorksForSelect', {
    extend: 'B4.base.Store',
    fields: ['Id', 'Name', 'YearRepair', 'Sum', 'TypeWork', 'UnitMeasure', 'Volume'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'GetListCrObjectWorksByObjectId'
    }
});
