Ext.define('B4.store.view.Decision', {
    requires: ['B4.model.Decision'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewDecisionStore',
    model: 'B4.model.Decision',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanDecision',
        listAction: 'ListView'
    }
});