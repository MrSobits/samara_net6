Ext.define('B4.model.manorg.ManOrgBilAdditionService', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgBilAdditionService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'BilService' },
        { name: 'ServiceName' },
        { name: 'MeasureName' }
    ]
});