Ext.define('B4.model.HouseClaims', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseClaims',
        listAction: 'OrderList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service' },
        { name: 'Total' },
        { name: 'Completed' },
        { name: 'InProgress' },
        { name: 'New' },
        { name: 'Overdue' }
    ]
});