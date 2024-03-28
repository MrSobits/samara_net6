/**
* стор тематик обращений
*/
Ext.define('B4.store.dict.StatSubjectGji', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubjectGji'],
    autoLoad: false,
    storeId: 'statSubjectGjiStore',
    model: 'B4.model.dict.StatSubjectGji'
});