Ext.define('B4.store.DistributionDetail', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Object' },
        { name: 'Sum' },
        { name: 'PaymentAccount' },
        { name: 'Destination' },
        { name: 'UserLogin' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'DistributionDetail',
        listAction: 'List'
    }
});