Ext.define('B4.store.realityobj.LiftTypeWorkCr', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectDetailCr',
        listAction: 'GetLiftTypeWorkCrDetail'
    },
    fields: [
        { name: 'ObjectCrId' },
        { name: 'Status' },
        { name: 'PeriodCr' },
        { name: 'CrPlan' },
        { name: 'YearRegProgram' },
        { name: 'GovCustomer' },
        { name: 'AuctionNumber' },
        { name: 'Builder' },
        { name: 'NumberGovContract' }
    ]
});