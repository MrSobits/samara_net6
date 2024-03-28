Ext.define('B4.store.disposal.RealityObjViolation', {
    extend: 'B4.base.Store',
    requires: ['B4.model.disposal.Violation'],
    autoLoad: false,
    storeId: 'disposalRealityObjViolationStore',
    model: 'B4.model.disposal.Violation',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalViol',
        listAction: 'ListRealityObject'
    }
});