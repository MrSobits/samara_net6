Ext.define('B4.model.licensing.FormGovernmentService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormGovernmentService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'GovernmentServiceType' },
        { name: 'Year' },
        { name: 'Quarter' }
    ]
});