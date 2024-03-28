Ext.define('B4.model.AuditLogMap', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: ['Id', 'Name'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'AuditLogMap',
        listAction: 'List'
    }
});
