Ext.define('B4.model.personalaccount.PublicControl', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicControlClaims',
        listAction: 'OrderList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CategoryName' },
        { name: 'Address' },
        { name: 'Territory' },
        { name: 'OrganizationName' },
        { name: 'StateName' },
        { name: 'CreatedDate' },
        { name: 'UpdateDate' }
    ]
});