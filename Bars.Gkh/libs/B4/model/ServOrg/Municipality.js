Ext.define('B4.model.servorg.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceOrgMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ServOrg', defaultValue: null },
        { name: 'Municipality', defaultValue: null }
    ]
});