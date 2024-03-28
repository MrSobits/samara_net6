Ext.define('B4.model.administration.ImportExportModel', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'TableName' },
        { name: 'Description' }
    ],
    proxy: {
        type: 'ajax',
        url: B4.Url.action('GetExportableEntitites', 'ImportExport'),
        reader: {
            type: 'json',
            root: 'data',
            totalProperty: 'totalCount'
        }
    }
});