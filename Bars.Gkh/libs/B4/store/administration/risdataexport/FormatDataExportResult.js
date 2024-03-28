Ext.define('B4.store.administration.risdataexport.FormatDataExportResult', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.risdataexport.FormatDataExportResult'],
    autoLoad: false,
    model: 'B4.model.administration.risdataexport.FormatDataExportResult',
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ]
});