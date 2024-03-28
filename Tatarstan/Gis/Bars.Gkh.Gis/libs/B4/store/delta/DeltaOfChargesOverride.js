/*
    Список начислений с группировкой
*/
Ext.define('B4.store.delta.DeltaOfChargesOverride', {
    extend: 'B4.base.Store',
    requires: ['B4.model.delta.DeltaOfChargesOverride'],
    autoLoad: false,
    model: 'B4.model.delta.DeltaOfChargesOverride',
    
    groupField: 'Topic' //группировочное поле
});