Ext.define('B4.model.objectcr.MonitoringSmr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MonitoringSmr'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCrId' },
        { name: 'State', defaultValue: null }
    ]
});