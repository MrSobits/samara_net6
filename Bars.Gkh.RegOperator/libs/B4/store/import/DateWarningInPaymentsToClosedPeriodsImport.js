/**
 * Хранилище предупреждений про даты при импорте оплат в закрытый период
 */
Ext.define('B4.store.import.DateWarningInPaymentsToClosedPeriodsImport', {
    extend: 'B4.base.Store',
    autoload: false,
    fields: [
        { name: 'Id' },
        { name: 'Title' },
        { name: 'Message' },
        { name: 'PaymentDate' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'DateWarningInPaymentsToClosedPeriodsImport',
        timeout: 5 * 60 * 1000 // 5 минут, с запасом на загрузку сервера под закрытие отчётного периода
    }
});