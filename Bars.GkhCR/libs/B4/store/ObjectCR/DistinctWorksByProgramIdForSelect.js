Ext.define('B4.store.objectcr.DistinctWorksByProgramIdForSelect', {
    extend: 'B4.base.Store',
    fields: ['Id', 'Name', 'TypeWork', 'UnitMeasure'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'GetListDistinctWorksByProgramId'
    }
});
