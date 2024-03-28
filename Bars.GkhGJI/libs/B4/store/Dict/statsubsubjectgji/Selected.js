/**
* стор подтематик обращения, выбранные записи
*/
Ext.define('B4.store.dict.statsubsubjectgji.Selected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubsubjectGji'],
    autoLoad: false,
    storeId: 'statSubsubjectGjiSelectedStore',
    model: 'B4.model.dict.StatSubsubjectGji'
});