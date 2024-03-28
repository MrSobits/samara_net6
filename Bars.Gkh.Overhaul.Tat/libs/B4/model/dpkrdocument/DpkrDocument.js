Ext.define('B4.model.dpkrdocument.DpkrDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentKind' },
        { name: 'DocumentKindName' },
        { name: 'DocumentName' },
        { name: 'File' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DocumentDepartment' },
        { name: 'PublicationDate' },
        { name: 'ObligationAfter2014' },
        { name: 'ObligationBefore2014' }
    ]
});