Ext.define('B4.model.actcheck.ActionViolation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckActionViolation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheckAction' },
        { name: 'Violation' },
        { name: 'ContrPersResponse' }
    ]
});