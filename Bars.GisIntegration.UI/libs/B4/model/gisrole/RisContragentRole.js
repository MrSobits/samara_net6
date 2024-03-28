Ext.define('B4.model.gisrole.RisContragentRole', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RisContragentRole'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Role', defaultValue: null },
        { name: 'Contragent', defaultValue: null }
    ]
});
