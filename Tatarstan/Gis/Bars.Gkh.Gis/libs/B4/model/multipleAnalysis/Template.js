Ext.define('B4.model.multipleAnalysis.Template', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MultipleAnalysisTemplate'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealEstateTypeId' },
        { name: 'RealEstateTypeName' },
        { name: 'TypeCondition' },
        { name: 'FormDay' },
        { name: 'Email' },
        { name: 'LastFormDate' },
        { name: 'MunicipalAreaGuid' },
        { name: 'MunicipalAreaName' },
        { name: 'SettlementGuid' },
        { name: 'SettlementName' },
        { name: 'StreetGuid' },
        { name: 'StreetName' },
        { name: 'MonthYear' }
    ]
});