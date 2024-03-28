Ext.define('B4.model.objectcr.ArchiveMultiplyContragentSmr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ArchiveMultiplyContragentSmr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeWorkCr' },
        { name: 'VolumeOfCompletion', defaultValue: 0 },
        { name: 'Contragent' },
        { name: 'PercentOfCompletion', defaultValue: 0 },
        { name: 'CostSum', defaultValue: 0 }
    ]
});