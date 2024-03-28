Ext.define('B4.model.dict.PlanInsCheckGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PlanInsCheckGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateApproval' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});