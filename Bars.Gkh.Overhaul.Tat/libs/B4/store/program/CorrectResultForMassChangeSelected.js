Ext.define('B4.store.program.CorrectResultForMassChangeSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.program.CorrectionResult'],
    autoLoad: false,
    model: 'B4.model.program.CorrectionResult',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrCorrectionStage2',
        listAction: 'ListForMassChangeYear'
    }
});