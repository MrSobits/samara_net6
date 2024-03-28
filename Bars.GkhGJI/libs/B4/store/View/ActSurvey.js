Ext.define('B4.store.view.ActSurvey', {
    requires: ['B4.model.ActSurvey'],
    extend: 'B4.base.Store',
    autoLoad: false,
    storeId: 'viewActCheckStore',
    model: 'B4.model.ActSurvey',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurvey',
        listAction: 'ListView'
    }
});