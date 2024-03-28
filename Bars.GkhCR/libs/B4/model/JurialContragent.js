Ext.define('B4.model.JurialContragent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});