Ext.define('B4.model.ConfirmContribution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConfirmContribution'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'ManagingOrganizationName' },
        { name: 'MunicipalityName' }
    ]
});