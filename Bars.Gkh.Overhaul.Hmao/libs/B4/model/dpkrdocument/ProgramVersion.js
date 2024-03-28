Ext.define('B4.model.dpkrdocument.ProgramVersion', {
    extend: 'B4.model.version.ProgramVersion',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrDocumentProgramVersion'
    }
});