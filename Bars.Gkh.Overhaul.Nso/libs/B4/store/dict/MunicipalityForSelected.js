Ext.define('B4.store.dict.MunicipalityForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Municipality'],
    autoLoad: false,
    storeId: 'municipalityForSelectedStore',
    model: 'B4.model.dict.Municipality'
});