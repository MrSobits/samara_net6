Ext.define('B4.model.regop.period.CloseChecker', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Impl' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PeriodCloseCheck',
        listAction: 'ListCheckers'
    }
});