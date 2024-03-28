Ext.define('B4.store.preventiveaction.task.ObjectiveForSelect', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Code' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectivesPreventiveMeasure'
    }
});