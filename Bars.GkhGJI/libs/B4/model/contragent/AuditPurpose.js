Ext.define('B4.model.contragent.AuditPurpose', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentAuditPurpose'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'AuditPurpose', defaultValue: null },
        { name: 'AuditPurposeId' },
        { name: 'EntityId' },
        { name: 'LastInspDate' },
        { name: 'AuditPurposeName' }
    ]
});