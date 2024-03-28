Ext.define('B4.model.dict.PlanJurPersonGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PlanJurPersonGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateApproval' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'DateDisposal' },
        { name: 'NumberDisposal' },
        { name: 'UriRegistrationNumber' }
    ]
});