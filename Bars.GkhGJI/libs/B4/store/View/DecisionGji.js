Ext.define('B4.store.view.DecisionGji', {
    requires: ['B4.model.DecisionGji'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewDecisionStore',
    model: 'B4.model.DecisionGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionGji',
        listAction: 'ListView'
    }
});