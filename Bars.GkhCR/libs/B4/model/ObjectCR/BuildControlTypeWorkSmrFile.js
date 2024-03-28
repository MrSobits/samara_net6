Ext.define('B4.model.objectcr.BuildControlTypeWorkSmrFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildControlTypeWorkSmrFile'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BuildControlTypeWorkSmr' },     
        { name: 'Description' },
        { name: 'VideoLink' },
        { name: 'ObjectCreateDate' },
        { name: 'FileInfo' }
    ]
});