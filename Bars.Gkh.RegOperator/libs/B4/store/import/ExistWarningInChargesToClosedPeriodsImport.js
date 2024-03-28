﻿/**
 * Хранилище предупреждений про существование начислений при импорте начислений в закрытый период
 */
Ext.define('B4.store.import.ExistWarningInChargesToClosedPeriodsImport', {
    extend: 'B4.base.Store',
    autoload: false,
    fields: [
        { name: 'Id' },
        { name: 'Title' },
        { name: 'Message' },
        { name: 'ChargeDescriptorName' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ExistWarningInChargesToClosedPeriodsImport',
        timeout: 5 * 60 * 1000 // 5 минут, с запасом на загрузку сервера под закрытие отчётного периода
    }
});