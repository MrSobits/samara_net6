Ext.define('B4.model.contractrf.Object', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractRfObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContractRf', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectName' },
        { name: 'MunicipalityName' },
        { name: 'ManOrgName' },
        { name: 'RealityObjectId' },
        { name: 'IncludeDate', defaultValue: null },
        { name: 'ExcludeDate', defaultValue: null },
        { name: 'TypeCondition', defaultValue: 10 },
        { name: 'GkhCode' },
        { name: 'RealityObjectAreaMkd' },
        { name: 'RealityObjectAreaLivingOwned' },
        { name: 'TotalArea' },
        { name: 'AreaLiving' },
        { name: 'AreaNotLiving' },
        { name: 'AreaLivingOwned' },
        { name: 'Note'}
    ]
});