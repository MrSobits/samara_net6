Ext.define('B4.model.objectcr.DefectList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DefectList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'Work' },
        { name: 'WorkName' },
        { name: 'DocumentName' },
        { name: 'DocumentDate' },
        { name: 'Sum', type: Ext.data.Types.FLOAT, useNull: true },
        { name: 'Volume' },
        { name: 'CostPerUnitVolume' },
        { name: 'State', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'TypeWork' },
        { name: 'CalculateBy' },
        { name: 'DpkrVolume' },
        { name: 'MargCost' },
        { name: 'DpkrCost' },
        { name: 'TypeDefectList', defaultValue: 0 },
        { name: 'SumTotal', type: Ext.data.Types.FLOAT, useNull: true, mapping: 'Sum' },
        { name: 'UsedInExport', defaultValue: 20 }
    ]
});