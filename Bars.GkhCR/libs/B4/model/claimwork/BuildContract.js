Ext.define('B4.model.claimwork.BuildContract', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'BuildContract' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'Contragent' },
        { name: 'Builder' },
        { name: 'Inn' },
        { name: 'DocumentNum' },
        { name: 'DocumentDateFrom' },
        { name: 'DateEndWork' },
        { name: 'CreationType', defaultValue : 20},
        { name: 'CountDaysDelay' },
        { name: 'ClaimWorkTypeBase' },
        { name: 'StartingDate' },
        { name: 'State' },
        { name: 'ObjCrId' },
        { name: 'Address' },
        { name: 'StateName' },
        { name: 'IsDebtPaid' },
        { name: 'DebtPaidDate' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildContractClaimWork',
        timeout: 5 * 60 * 1000
    }
});
