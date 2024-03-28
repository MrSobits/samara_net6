Ext.define('B4.store.shortprogram.WorkSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.shortprogram.Work'],
    autoLoad: false,
    model: 'B4.model.shortprogram.Work',
    proxy: {
        type: 'b4proxy',
        controllerName : 'ShortProgramRecord',
        listAction: 'ListWork'
    }
});