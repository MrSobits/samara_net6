Ext.define('B4.model.specialobjectcr.MonitoringSmr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialMonitoringSmr'
    },
    fields: [
        { name: 'Id' },
        { name: 'ObjectCrId' },
        { name: 'State', defaultValue: null }
    ]
});