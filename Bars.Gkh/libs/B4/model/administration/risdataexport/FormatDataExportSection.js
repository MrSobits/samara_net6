Ext.define('B4.model.administration.risdataexport.FormatDataExportSection', {
    extend: 'B4.base.Model',
    idProperty: 'Code',
    fields: [
        { name: 'Code' },
        { name: 'Description' },
        { name: 'InheritedEntityCodeList' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormatDataExport',
        listAction: 'ListAvailableSection'
    },
});