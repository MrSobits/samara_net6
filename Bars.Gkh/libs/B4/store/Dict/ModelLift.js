Ext.define('B4.store.dict.ModelLift', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ModelLift'],
    autoLoad: false,
    storeId: 'modelLiftStore',
    model: 'B4.model.dict.ModelLift'
});