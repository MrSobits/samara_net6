Ext.define('B4.store.manorg.ReceptionCitizens', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.WorkMode'],
    autoLoad: false,
    storeId: 'managingReceptionCitizensStore',
    model: 'B4.model.manorg.WorkMode'
});