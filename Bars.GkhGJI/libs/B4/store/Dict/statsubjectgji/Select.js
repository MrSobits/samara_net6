/**
* стор тематик обращения, для выбора
*/
Ext.define('B4.store.dict.statsubjectgji.Select', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubjectGji'],
    autoLoad: false,
    storeId: 'statSubjectGjiSelectStore',
    model: 'B4.model.dict.StatSubjectGji'
});