Ext.define('B4.model.preventivevisit.ResultViolation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisitResultViolation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PreventiveVisitResult', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'Pprf' },
        { name: 'ViolationGjiName' },
        { name: 'CodePin' }
    ]
});