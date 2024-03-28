Ext.define('B4.model.regop.suspense.ObjectCrForSuspense', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'MethodFormFund' },
        { name: 'Address' },
        { name: 'FundDeficiency' },
        { name: 'Sum' }
    ],
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'SuspenseAccount',
        listAction: 'ObjectCrForDistribution'
    }
});