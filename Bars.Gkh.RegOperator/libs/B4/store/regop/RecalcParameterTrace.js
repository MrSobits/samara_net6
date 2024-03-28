Ext.define('B4.store.regop.RecalcParameterTrace', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Period' },
        { name: 'Tariff' },
        { name: 'Share' },
        { name: 'RoomArea' },
        { name: 'Recalc_Charge' },
        { name: 'Fact_Charge' },
        { name: 'Summary' },
        { name: 'CountDays' },
        { name: 'TariffSource' },
        { name: 'DateActualShare' },
        { name: 'DateActualArea' },
        { name: 'RecalcHistory' },
        { name: 'CurrentCharge' },
        { name: 'PeriodName' },
        { name: 'RecalcReason' },
        { name: 'RecalcReasonDate' },
        { name: 'RecalcReasonValue' },
        { name: 'CountDaysMonth' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountPeriodSummary',
        listAction: 'ListReCalcParameterTrace'
    }
});