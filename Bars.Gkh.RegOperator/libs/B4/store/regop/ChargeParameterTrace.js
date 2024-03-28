Ext.define('B4.store.regop.ChargeParameterTrace', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Period' },
        { name: 'Tariff' },
        { name: 'Share' },
        { name: 'RoomArea' },
        { name: 'OpenDate' },
        { name: 'Summary' },
        { name: 'CountDays' },
        { name: 'TariffSource' },
        { name: 'DateActualShare' },
        { name: 'DateActualArea' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountPeriodSummary',
        listAction: 'ListChargeParameterTrace'
    }
});