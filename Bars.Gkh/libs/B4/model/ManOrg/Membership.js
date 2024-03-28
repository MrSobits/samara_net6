Ext.define('B4.model.manorg.Membership', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrgMembership'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'Address' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Name' },
        { name: 'DocumentNum' },
        { name: 'OfficialSite' }
    ]
});