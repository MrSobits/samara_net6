Ext.define('B4.model.regop.period.CloseCheck', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Impl' },
        { name: 'IsCritical' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PeriodCloseCheck'
    }
});