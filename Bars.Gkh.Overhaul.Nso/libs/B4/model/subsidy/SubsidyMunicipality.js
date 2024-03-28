Ext.define('B4.model.subsidy.SubsidyMunicipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SubsidyMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'StartTarif' },
        { name: 'CoefGrowthTarif' },
        { name: 'CoefSumRisk' },
        { name: 'DateReturnLoan' },
        { name: 'CoefAvgInflationPerYear' },
        { name: 'ConsiderInflation' }
    ]
});