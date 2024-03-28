Ext.define('B4.store.regop.RecalcPenaltyTrace', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Period'},
        { name: 'Percentage' },
        { name: 'CountDays' },
        { name: 'RecalcPenalty' },
        { name: 'Penalty' },
        { name: 'Summary' },
        { name: 'CalcType' },
        { name: 'CalcDecision' },
        { name: 'Payment' },
        { name: 'PaymentDate' },
        { name: 'RecalcReason' },
        { name: 'RecalcDate' },
        { name: 'CalculationGuid' },
        { name: 'RecalcHistory' },
        { name: 'CurrentPenalty' },
        { name: 'PeriodName' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountPeriodSummary',
        listAction: 'ListRecalcPenaltyTrace'
    }
});