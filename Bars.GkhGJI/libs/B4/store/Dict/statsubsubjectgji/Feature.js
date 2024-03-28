/**
* стор характеристик нарушений, связанных с подтематикой
*/
Ext.define('B4.store.dict.statsubsubjectgji.Feature', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubsubjectFeatureGji'],
    autoLoad: false,
    storeId: 'statSubsubjectFeatureGjiStore',
    model: 'B4.model.dict.StatSubsubjectFeatureGji'
});