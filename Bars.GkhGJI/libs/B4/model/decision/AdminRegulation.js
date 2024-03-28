Ext.define('B4.model.decision.AdminRegulation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionAdminRegulation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'Name' },
        { name: 'AdminRegulation' },
        { name: 'Code' }
    ]
});