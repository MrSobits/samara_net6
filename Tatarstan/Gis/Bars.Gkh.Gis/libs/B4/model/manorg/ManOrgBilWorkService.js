Ext.define('B4.model.manorg.ManOrgBilWorkService', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgBilWorkService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'BilService' }, 
        { name: 'ServiceName' },
        { name: 'MeasureName' },
        { name: 'ServiceCode' },
        { name: 'Purpose' },
        { name: 'Type' },
        { name: 'Description' }
    ]
});