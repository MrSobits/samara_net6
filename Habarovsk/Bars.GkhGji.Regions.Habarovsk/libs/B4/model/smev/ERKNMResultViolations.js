Ext.define('B4.model.smev.ERKNMResultViolations', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ERKNMResultViolations'
    },
    fields: [
        { name: 'Id'},
        { name: 'ERKNM' },
        { name: 'VIOLATION_NOTE' },
        { name: 'VIOLATION_ACT' },
        { name: 'TEXT' },
        { name: 'NUM_GUID' },
        { name: 'VLAWSUIT_TYPE_ID' },
        { name: 'CODE' },
        { name: 'DATE_APPOINTMENT' },
        { name: 'EXECUTION_DEADLINE' },
        { name: 'EXECUTION_NOTE' }
    ]
});