Ext.define('B4.model.workactregister.Details', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkAct'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'WorkName' },
        { name: 'PlanVolume' },
        { name: 'PlanSum' },
        { name: 'ActVolume' },
        { name: 'ActSum' },
        { name: 'CompleteVolumePercent' },
        { name: 'UsedResourcesPercent' },
        { name: 'TypeWorkLimit' },

        //summary
        { name: 'PlanTotalVolume' },
        { name: 'PlanTotalSum' },
        { name: 'ActTotalVolume' },
        { name: 'ActTotalSum' },
        { name: 'TypeWorkTotalLimit' },
        { name: 'CompleteVolumeAvgPercent' },
        { name: 'UsedResourcesAvgPercent' }
    ]
});