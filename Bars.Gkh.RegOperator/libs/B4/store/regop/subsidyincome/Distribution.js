Ext.define('B4.store.regop.subsidyincome.Distribution', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Route', 'Name', 'Code'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'SubsidyIncome',
        listAction: 'ListSubsidyDistribution'
    }
});