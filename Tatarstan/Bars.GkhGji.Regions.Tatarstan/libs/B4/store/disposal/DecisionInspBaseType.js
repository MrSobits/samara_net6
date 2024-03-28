Ext.define('B4.store.disposal.DecisionInspBaseType', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.InspectionBaseType'],
    autoLoad: false,
    model: 'B4.model.dict.InspectionBaseType',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionInspectionBase',
        listAction: 'ListInspectionBaseType'
    }
});