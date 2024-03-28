Ext.define('B4.model.PlanWorkServiceRepair', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PlanWorkServiceRepair'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'Name' },
        { name: 'ProviderName' }
    ]
});