Ext.define('B4.model.claimwork.restructdebt.Schedule', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'PersonalAccountNum' },
        { name: 'TotalDebtSum' },
        { name: 'PlanedPaymentDate' },
        { name: 'PlanedPaymentSum' },
        { name: 'PaymentDate' },
        { name: 'PaymentSum' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RestructDebtSchedule'
    }
});