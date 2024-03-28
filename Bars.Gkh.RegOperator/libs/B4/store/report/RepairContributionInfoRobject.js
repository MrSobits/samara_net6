Ext.define('B4.store.report.RepairContributionInfoRobject', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Address' },
        { name: 'Municipality' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOpReport',
        listAction: 'RepairContributionInfoRobjectList'
    }
});