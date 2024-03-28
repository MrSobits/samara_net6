Ext.define('B4.model.manorg.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrgMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManOrg', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'ParentMo', defaultValue: null }
    ]
});