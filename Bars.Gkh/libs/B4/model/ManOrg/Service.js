Ext.define('B4.model.manorg.Service', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrgService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'Name' },
        { name: 'Work' },
        { name: 'WorkName'}
    ]
});