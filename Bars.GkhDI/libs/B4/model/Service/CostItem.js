Ext.define('B4.model.service.CostItem', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CostItem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'Name' },
        { name: 'Count', defaultValue: null },
        { name: 'Sum', defaultValue: null },
        { name: 'Cost' }
    ]
});