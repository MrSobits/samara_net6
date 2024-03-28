Ext.define('B4.store.administration.risdataexport.FormatDataExportInfo', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.risdataexport.FormatDataExportInfo'],
    autoLoad: false,
    model: 'B4.model.administration.risdataexport.FormatDataExportInfo',
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ]
});