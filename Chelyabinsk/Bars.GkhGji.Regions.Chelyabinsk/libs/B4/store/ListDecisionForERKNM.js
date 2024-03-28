Ext.define('B4.store.ListDecisionForERKNM', {
    extend: 'B4.base.Store',
    requires: ['B4.model.DocumentGji'],
    autoLoad: false,
    model: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ERKNMExecute',
        listAction: 'GetListDecision'
    }
});