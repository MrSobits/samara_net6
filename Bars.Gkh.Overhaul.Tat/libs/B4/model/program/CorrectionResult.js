Ext.define('B4.model.program.CorrectionResult', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrCorrectionStage2',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'CommonEstateObjectName' },
        { name: 'IndexNumber' },
        { name: 'Point' },
        { name: 'Sum' },
        { name: 'PlanYear' },
        { name: 'FirstPlanYear' },
        { name: 'TypeResult' },
        { name: 'PublishYear' },
        { name: 'PublishYearExceeded' },
        { name: 'St3Id' },
        { name: 'FixedYear' }
    ]
});