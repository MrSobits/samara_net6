Ext.define('B4.model.objectcr.DesignAssignment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DesignAssignment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'Document' },
        { name: 'Date', defaultValue: null},
        { name: 'TypeWorksCr' },
        { name: 'State', defaultValue: null },
        { name: 'DocumentFile', defaultValue: null },
        { name: 'UsedInExport', defaultValue: 20 }
    ]
});