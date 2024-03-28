Ext.define('B4.store.dpkrdocument.ProgramVersionSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dpkrdocument.ProgramVersion'],
    autoLoad: false,
    model: 'B4.model.dpkrdocument.ProgramVersion',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrDocumentProgramVersion',
        listAction: 'GetProgramVersionList'
    }
});