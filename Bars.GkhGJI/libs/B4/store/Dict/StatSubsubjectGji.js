/**
* стор подтематик обращений
*/
Ext.define('B4.store.dict.StatSubsubjectGji', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StatSubsubjectGji'],
    autoLoad: false,
    storeId: 'statSubsubjectGjiStore',
    model: 'B4.model.dict.StatSubsubjectGji'
});