Ext.define('B4.model.manorg.Claim', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrgClaim'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'AmountClaim'},
        { name: 'ContentClaim' },
        { name: 'DateClaim' }
    ]
});