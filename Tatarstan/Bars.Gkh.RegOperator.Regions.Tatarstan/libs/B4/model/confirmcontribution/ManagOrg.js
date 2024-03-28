Ext.define('B4.model.confirmcontribution.ManagOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConfirmContributionManagOrg',
        listAction: 'ListManagOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganizationName' }
    ]
});