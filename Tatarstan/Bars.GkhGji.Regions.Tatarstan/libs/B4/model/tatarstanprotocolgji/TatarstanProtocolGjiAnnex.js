Ext.define('B4.model.tatarstanprotocolgji.TatarstanProtocolGjiAnnex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanProtocolGjiAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentGji' },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File' }
    ]
});