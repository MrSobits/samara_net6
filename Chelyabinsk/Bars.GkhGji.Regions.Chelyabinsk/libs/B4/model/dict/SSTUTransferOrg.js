Ext.define('B4.model.dict.SSTUTransferOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SSTUTransferOrg'
    },
    fields: [
        { name: 'Name' },
        { name: 'Description' },
        { name: 'Guid' }
    ]
});