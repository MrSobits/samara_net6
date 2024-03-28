Ext.define('B4.model.MembershipUnions', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MembershipUnions'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Address' },
        { name: 'OfficialSite' }
    ]
});