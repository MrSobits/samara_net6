Ext.define('B4.store.objectcr.TypeWorkStage1', {
    extend: 'B4.base.Store',
    fields: [
        'Id',
        'StructuralElement',
        'Volume',
        'Sum',
        'Year',
        'TypeWorkCr',
        'WorkName'
    ],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCr',
        listAction: 'TypeWorkStage1List'
    }
});