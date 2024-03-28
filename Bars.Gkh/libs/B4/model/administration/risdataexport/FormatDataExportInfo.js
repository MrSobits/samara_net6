Ext.define('B4.model.administration.risdataexport.FormatDataExportInfo', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'State' },
        { name: 'ObjectType' },
        { name: 'LoadDate' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormatDataExportInfo'
    }
});