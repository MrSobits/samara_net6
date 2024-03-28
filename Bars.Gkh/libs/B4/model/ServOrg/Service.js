Ext.define('B4.model.servorg.Service', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceOrgService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ServiceOrganization', defaultValue: null },
        { name: 'TypeService' }
    ]
});