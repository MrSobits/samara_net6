/**
* Хранилище задач на сервере вычислений, которые сейчас разбирают импорты начислений
*/
Ext.define('B4.store.import.closedperiodsimport.RunningTasks', {
    extend: 'B4.base.Store',
    autoload: false,
    fields: [
        { name: 'ObjectCreateDate' },
        { name: 'Percentage' },
        { name: 'Status', type: { type: "auto", isEnum: true, enumName: "TaskStatus" }, defaultValue: 0 }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ClosedPeriodsImport',
        listAction: 'RunningTasksList'
    }
});