Ext.define('B4.model.administration.ImportExportLogModel', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'FileInfo' },
        { name: 'Type' },
        { name: 'HasErrors' },
        { name: 'HasMessages' },
        { name: 'DateStart' }
    ],
    proxy: {
        type: 'ajax',
        url: B4.Url.action('getlog', 'importexport'),
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});