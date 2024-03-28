Ext.define('B4.model.finactivity.Audit', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityAudit'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'TypeAuditStateDi', defaultValue: 10 },
        { name: 'Year' },
        { name: 'File', defaultValue: null }
    ]
});