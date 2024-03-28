Ext.define('B4.store.DecisionNoticeRegister', {
    extend: 'B4.base.Store',
    requires: ['B4.model.SpecialAccountDecisionNotice'],
    autoLoad: false,
    model: 'B4.model.SpecialAccountDecisionNotice',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccountDecisionNotice',
        listAction: 'ListRegister'
    }
});