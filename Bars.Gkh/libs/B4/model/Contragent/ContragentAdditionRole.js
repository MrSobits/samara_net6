Ext.define('B4.model.contragent.ContragentAdditionRole', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentAdditionRole'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});