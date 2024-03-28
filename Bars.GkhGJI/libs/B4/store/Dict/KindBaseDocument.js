//модель добавляется в ResourceManifest.Part
Ext.define('B4.store.dict.KindBaseDocument', {
    extend: 'B4.base.Store',
    requires: ['B4.model.KindBaseDocument'],
    autoLoad: false,
    model: 'B4.model.KindBaseDocument'
});