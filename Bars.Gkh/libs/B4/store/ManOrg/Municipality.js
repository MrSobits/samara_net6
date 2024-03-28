Ext.define('B4.store.manorg.Municipality', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.Municipality'],
    autoLoad: false,
    storeId: 'manorgMunicipalityStore',
    model: 'B4.model.manorg.Municipality'
});