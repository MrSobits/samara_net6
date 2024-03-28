Ext.define('B4.model.regop.period.CloseCheckResult',
{
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'IsCritical' },
        { name: 'CheckState' },
        { name: 'CheckDate' },
        { name: 'LogFile' },
        { name: 'Name' },
        { name: 'Note' },
        { name: 'PersAccGroup' },
        { name: 'User' },
        { name: 'FullLogFile' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PeriodCloseCheckResult'
    }
});