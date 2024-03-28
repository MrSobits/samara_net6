Ext.define('B4.model.efficiencyrating.ManagingOrganizationDataValue', {
    extend: 'B4.model.metavalueconstructor.BaseDataValue',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrganizationDataValue'
    },
    fields: [
        { name: 'EfManagingOrganization' },
        { name: 'Dynamics' }
    ]
});