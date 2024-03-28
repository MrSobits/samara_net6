Ext.define('B4.model.regop.loan.Manage', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'Address' },
        { name: 'WorkNames' },
        { name: 'Year' },
        { name: 'Collection' },
        { name: 'NeedSum' },
        { name: 'OwnerSum' },
        { name: 'OwnerLoanSum' },
        { name: 'SubsidySum' },
        { name: 'OtherSum' },
        { name: 'CalcAccountNumber' },
        { name: 'Task' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLoan',
        listAction: 'ListLoansByProgramAndMunicipality',
        timeout: 5 * 60 * 1000 // 5 минут
    }
});