Ext.define('B4.model.objectcr.BuildControlTypeWorkSmr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildControlTypeWorkSmr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeWorkCr' },
        { name: 'VolumeOfCompletion', defaultValue: 0 },
        { name: 'Contragent' },
        { name: 'PercentOfCompletion', defaultValue: 0 },
        { name: 'CostSum', defaultValue: 0 },
        { name: 'Latitude', defaultValue: 0 },
        { name: 'Longitude', defaultValue: 0 },
        { name: 'Description' },
        { name: 'Controller' },
        { name: 'DeadlineMissed', defaultValue: false },
        { name: 'TypeWorkCrAddWork' },
        { name: 'ObjectCreateDate' },
        { name: 'MonitoringDate' }
    ]
});