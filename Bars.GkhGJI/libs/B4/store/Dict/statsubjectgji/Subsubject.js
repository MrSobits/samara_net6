/**
* стор подтематик обращения, связанных с тематикой
*/
Ext.define('B4.store.dict.statsubjectgji.Subsubject', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubjectSubsubjectGji'],
    autoLoad: false,
    storeId: 'statSubjectSubsubjectGjiStore',
    model: 'B4.model.dict.StatSubjectSubsubjectGji'
});