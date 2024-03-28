Ext.define('B4.store.administration.ImportExportStore', {    
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.ImportExportModel'],
    model: 'B4.model.administration.ImportExportModel',
    autoLoad: true,
    pageSize: 100
});