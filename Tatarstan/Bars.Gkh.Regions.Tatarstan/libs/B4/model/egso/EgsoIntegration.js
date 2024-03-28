Ext.define('B4.model.egso.EgsoIntegration', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EgsoIntegration',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TaskType', defaultValue: null },
        { name: 'StateType', defaultValue: null },
        { name: 'User', defaultValue: null },
        { name: 'ObjectCreateDate', type: 'date' },
        { name: 'EndDate', type: 'date', defaultValue: null },
        { name: 'Year', defaultValue: null },
        { name: 'LogId', defaultValue: null }
    ]
});