Ext.define('B4.store.localgov.ReceptionCitizens', {
    extend: 'B4.base.Store',
    requires: ['B4.model.localgov.WorkMode'],
    autoLoad: false,
    storeId: 'localGovernmentReceptionCitizens',
    model: 'B4.model.localgov.WorkMode'
});