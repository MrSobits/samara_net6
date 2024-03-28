Ext.define('B4.store.DistributionType', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Route', 'Name', 'Code', 'DistributableAutomatically'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'SuspenseAccount',
        listAction: 'ListDistribution'
    }
});