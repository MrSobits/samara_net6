Ext.define('B4.store.dict.Work', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Work'],
    model: 'B4.model.dict.Work',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Defect',
        listAction: 'WorksForDefectList'
    }
});