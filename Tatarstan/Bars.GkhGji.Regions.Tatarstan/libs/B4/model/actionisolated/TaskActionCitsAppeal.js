Ext.define('B4.model.actionisolated.TaskActionCitsAppeal', {
    extend: 'B4.model.actionisolated.TaskAction',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskActionIsolated',
        listAction: 'ListForCitizenAppeal'
    }
});