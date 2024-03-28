Ext.define('B4.model.DocumentsRealityObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentsRealityObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'FileActState', defaultValue: null },
        { name: 'DescriptionActState' },
        { name: 'FileCatalogRepair', defaultValue: null },
        { name: 'FileReportPlanRepair', defaultValue: null },
        { name: 'HasGeneralMeetingOfOwners', defaultValue: 30}
    ]
});