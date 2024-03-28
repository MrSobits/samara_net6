Ext.define('B4.store.administration.risdataexport.FormatDataExportTask', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.risdataexport.FormatDataExportTask'],
    autoLoad: false,
    model: 'B4.model.administration.risdataexport.FormatDataExportTask',
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ]
});