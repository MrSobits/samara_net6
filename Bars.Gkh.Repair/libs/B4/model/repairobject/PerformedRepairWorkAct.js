Ext.define('B4.model.repairobject.PerformedRepairWorkAct', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedRepairWorkAct'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectAddress' },
        { name: 'RepairWork', defaultValue: null },
        { name: 'WorkName' },
        { name: 'PerformedWorkVolume' },
        { name: 'ActSum' },
        { name: 'ObjectPhoto', defaultValue: null },
        { name: 'ObjectPhotoDescription' },
        { name: 'ActDate' },
        { name: 'ActNumber' },
        { name: 'ActFile', defaultValue: null },
        { name: 'ActDescription' }
    ]
});