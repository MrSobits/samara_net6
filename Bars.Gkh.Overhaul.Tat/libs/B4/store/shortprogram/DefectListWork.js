Ext.define('B4.store.shortprogram.DefectListWork', {
    extend: 'B4.base.Store',
    fields : ['Id', 'Name'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramDefectList',
        listAction: 'GetWorks'
    }
});