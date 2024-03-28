Ext.define('B4.store.realityobj.decisionhistory.JobYears', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'JobName' },
        { name: 'UserYear' },
        { name: 'PlanYear' },
        { name: 'Protocol' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'Decision',
        listAction: 'GetJobYearsHistory'
    }
});