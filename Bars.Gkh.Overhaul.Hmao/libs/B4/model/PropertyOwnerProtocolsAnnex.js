Ext.define('B4.model.PropertyOwnerProtocolsAnnex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PropertyOwnerProtocolsAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Description' },
        { name: 'FileInfo', defaultValue: null },
        { name: 'Protocol' },
        { name: 'DocumentDate' },
        { name: 'Name' }
    ]
});