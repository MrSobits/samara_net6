Ext.define('B4.store.regop.PenaltyParameterTrace', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Period' },
        { name: 'Percentage' },
        { name: 'Reason' },
        { name: 'PenaltyDebt' },
        { name: 'Summary' },
        { name: 'CountDays' },
        { name: 'CalcType' },
        { name: 'CalcDecision' },
        { name: 'Payment' },
        { name: 'PaymentDate' },
        { name: 'PenaltyDate' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountPeriodSummary',
        listAction: 'ListPenaltyParameterTrace'
    }
});