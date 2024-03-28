Ext.define('B4.model.subsidy.DefaultPlanCollectionInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DefaultPlanCollectionInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Year' },
        { name: 'PlanOwnerPercent' },
        { name: 'NotReduceSizePercent' }
    ]
});