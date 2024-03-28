Ext.define('B4.model.longtermprobject.ListServicesWorksModel', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ListServiceDecisionWorkPlan'
    },
    fields: [
        { name: 'Id' },
        { name: 'Work' },
        { name: 'FactYear' },
        { name: 'PlanYear' }
    ]
});