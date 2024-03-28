/**
 * Хранилище предупреждений про лицевые счета при импорте оплат в закрытый период
 */
Ext.define('B4.store.import.AccountWarningInPaymentsToClosedPeriodsImport', {
    extend: 'B4.base.Store',
    autoload: false,
    fields: [
        { name: 'Id' },
        { name: 'Title' },
        { name: 'Message' },
        { name: 'InnerNumber' },
        { name: 'ExternalNumber' },
        { name: 'InnerRkcId' },
        { name: 'ExternalRkcId' },
        { name: 'Name' },
        { name: 'Address' },                
        { name: 'IsProcessed' },
        { name: 'IsCanAutoCompared' },
        { name: 'ComparingInfo' }
    ],    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AccountWarningInPaymentsToClosedPeriodsImport',        
        timeout: 5 * 60 * 1000 // 5 минут, с запасом на загрузку сервера под закрытие отчётного периода
    }
});