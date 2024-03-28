Ext.define('B4.model.administration.risdataexport.FormatDataExportEntity', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'ErrorMessage' },
        { name: 'ExportDate' },
        { name: 'ExportEntityState' },
        { name: 'DpkrName' },
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentKind' },
        { name: 'Address' },
        { name: 'Locality' },
        { name: 'StructElName' },
        { name: 'PublishedYear' },
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormatDataExportEntity'
    }
});