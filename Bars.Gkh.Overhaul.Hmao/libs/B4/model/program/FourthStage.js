Ext.define('B4.model.program.FourthStage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrCorrectionStage2'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'CommonEstateObjectName' },
        { name: 'Year' },
        { name: 'IndexNumber' },
        { name: 'Point' },
        { name: 'Sum' },
        { name: 'PlanYear' },
        { name: 'CorrectionYear' }
    ]
});