Ext.define('B4.model.actcheck.ActionFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckActionFile'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheckAction' },
        { name: 'Name' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'File' }
    ]
});