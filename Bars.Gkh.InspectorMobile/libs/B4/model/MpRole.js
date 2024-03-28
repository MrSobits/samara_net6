Ext.define('B4.model.MpRole', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MpRole'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});