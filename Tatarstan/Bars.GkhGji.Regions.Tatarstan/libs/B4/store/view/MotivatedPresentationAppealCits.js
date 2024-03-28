Ext.define('B4.store.view.MotivatedPresentationAppealCits', {
    requires: ['B4.model.MotivatedPresentationAppealCits'],
    extend: 'B4.base.Store',
    autoLoad: false,
    model: 'B4.model.MotivatedPresentationAppealCits',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivatedPresentationAppealCits',
        listAction: 'ListForRegistry'
    }
});