Ext.define('B4.store.finactivity.DocByYear', {
    extend: 'B4.base.Store',
    requires: ['B4.model.finactivity.DocByYear'],
    autoLoad: false,
    storeId: 'finActivityDocByYearStore',
    model: 'B4.model.finactivity.DocByYear',
    groupField: 'TypeDocByYearDi'
});