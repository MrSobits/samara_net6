Ext.define('B4.store.desktop.TaskState', {
    extend: 'B4.base.Store',
    fields: ['TypeReminder', 'CountRed', 'CountYellow', 'CountGreen', 'CountWhite'],
    autoLoad: false,
    storeId: 'storeWidgetTaskState',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListTaskState'
    }
});