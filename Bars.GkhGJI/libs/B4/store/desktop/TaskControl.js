Ext.define('B4.store.desktop.TaskControl', {
    extend: 'B4.base.Store',
    fields: ['InspectorFio', 'InspectorId', 'CountRed', 'CountYellow', 'CountGreen', 'CountWhite'],
    autoLoad: false,
    storeId: 'storeWidgetTaskControl',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'Reminder',
        listAction: 'ListTaskControl'
    }
});