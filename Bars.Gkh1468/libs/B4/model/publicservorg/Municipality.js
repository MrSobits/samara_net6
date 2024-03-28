Ext.define('B4.model.publicservorg.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicServiceOrgMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PublicServiceOrg' },
        { name: 'Municipality' }
    ]
});

