/**
* стор тематик обращения, связанных с подтематикой
*/
Ext.define('B4.store.dict.statsubsubjectgji.Subject', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubjectSubsubjectGji'],
    autoLoad: false,
    storeId: 'statSubsubjectSubjectGjiStore',
    model: 'B4.model.dict.StatSubjectSubsubjectGji'
});