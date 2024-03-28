/**
* стор тематик обращения, выбранные
*/
Ext.define('B4.store.dict.statsubjectgji.Selected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubjectGji'],
    autoLoad: false,
    storeId: 'statSubjectGjiSelectedStore',
    model: 'B4.model.dict.StatSubjectGji'
});