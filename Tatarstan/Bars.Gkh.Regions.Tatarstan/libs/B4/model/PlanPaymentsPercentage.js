Ext.define('B4.model.PlanPaymentsPercentage', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'PublicServiceOrg' },
        { name: 'Service' },
        { name: 'Resource' },
        { name: 'Percentage' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PlanPaymentsPercentage'
    }
});