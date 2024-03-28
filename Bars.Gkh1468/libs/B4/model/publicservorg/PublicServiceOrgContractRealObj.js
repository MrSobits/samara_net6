Ext.define('B4.model.publicservorg.PublicServiceOrgContractRealObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicServiceOrgContractRealObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'ManOrgs' },
        { name: 'Address' }
    ]
});