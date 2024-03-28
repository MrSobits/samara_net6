Ext.define('B4.store.disposal.DisposalControlObjectKind', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ControlObjectKind'],
    autoLoad: false,
    model: 'B4.model.dict.ControlObjectKind',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionControlObjectInfo',
        listAction: 'ListControlObjectKind'
    }
});