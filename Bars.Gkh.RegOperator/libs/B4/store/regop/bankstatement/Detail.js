Ext.define('B4.store.regop.bankstatement.Detail', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Object' },
        { name: 'Sum' },
        { name: 'DistributionType' },
        { name: 'Destination' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: '',
        listAction: ''
    }
});