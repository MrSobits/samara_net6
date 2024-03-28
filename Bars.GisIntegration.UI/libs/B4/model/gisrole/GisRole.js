Ext.define('B4.model.gisrole.GisRole', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisRole'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});
