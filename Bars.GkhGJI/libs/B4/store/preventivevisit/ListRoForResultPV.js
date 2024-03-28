Ext.define('B4.store.preventivevisit.ListRoForResultPV', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisitOperations',
        listAction: 'GetListRoForResultPV'
    }
});