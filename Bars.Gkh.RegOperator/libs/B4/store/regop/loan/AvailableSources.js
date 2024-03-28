Ext.define('B4.store.regop.loan.AvailableSources', {
    extend: 'B4.base.Store',
    fields: [
        { name: 'TypeSource' },
        { name: 'Name' },
        { name: 'AvailableMoney' },
        { name: 'TakenMoney' }
    ],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLoan',
        listAction: 'ListAvailableSources',
        timeout: 15 * 60 * 1000
    }
});