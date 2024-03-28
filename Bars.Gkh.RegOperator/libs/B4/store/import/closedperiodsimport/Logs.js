﻿/**
 * Хранилище журнала импортов начислений
 */
Ext.define('B4.store.import.closedperiodsimport.Logs', {
    extend: 'B4.base.Store',
    autoload: false,
    fields: [
        { name: 'Id', useNull: true },        
        { name: 'ObjectCreateDate' },
        { name: 'FileName' },
        { name: 'CountError' },
        { name: 'CountWarning' }
    ],
    sorters: [{ property: 'ObjectCreateDate', direction: 'DESC' }], // Обратная сортировка - последние импорты на первой странице
    proxy: {
        type: 'b4proxy',
        controllerName: 'ClosedPeriodsImport',
        listAction: 'LogsList'
    }
});