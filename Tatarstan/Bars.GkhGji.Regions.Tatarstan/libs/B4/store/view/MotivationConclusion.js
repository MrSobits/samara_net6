Ext.define('B4.store.view.MotivationConclusion', {
    requires: ['B4.model.MotivationConclusion'],
    extend: 'B4.base.Store',
    autoLoad: false,
    model: 'B4.model.MotivationConclusion',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivationConclusion',
        listAction: 'ListView'
    }
});