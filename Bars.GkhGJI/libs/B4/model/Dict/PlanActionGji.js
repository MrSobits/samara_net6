Ext.define('B4.model.dict.PlanActionGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PlanActionGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});