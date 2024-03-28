Ext.define('B4.model.regop.period.CloseCheckHistory', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'ChangeDate' },
        { name: 'IsCritical' },
        { name: 'User' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PeriodCloseCheckHistory'
    }
});