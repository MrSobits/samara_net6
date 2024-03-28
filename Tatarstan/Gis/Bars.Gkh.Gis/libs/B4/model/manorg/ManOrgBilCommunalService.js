Ext.define('B4.model.manorg.ManOrgBilCommunalService', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgBilCommunalService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'BilService' },
        { name: 'ServiceName' },
        { name: 'Name' },
        { name: 'Resource' },
        { name: 'IsOdnService' },
        { name: 'OrderNumber' }
    ]
});