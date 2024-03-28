/**
* стор подтематик обращения, для выбора
*/
Ext.define('B4.store.dict.statsubsubjectgji.Select', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubsubjectGji'],
    autoLoad: false,
    storeId: 'statSubsubjectGjiSelectStore',
    model: 'B4.model.dict.StatSubsubjectGji'
});