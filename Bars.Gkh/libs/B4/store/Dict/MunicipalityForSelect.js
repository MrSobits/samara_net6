Ext.define('B4.store.dict.MunicipalityForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Municipality'],
    autoLoad: false,
    storeId: 'municipalityForSelectStore',
    model: 'B4.model.dict.Municipality'
});