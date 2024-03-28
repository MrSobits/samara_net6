Ext.define('B4.store.dict.UnitMeasureNoPaging', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.UnitMeasure'],
    autoLoad: false,
    model: 'B4.model.dict.UnitMeasure',
    proxy: {
        type: 'b4proxy',
        listAction: 'ListNoPaging',
        controllerName: 'UnitMeasure'
    }
});