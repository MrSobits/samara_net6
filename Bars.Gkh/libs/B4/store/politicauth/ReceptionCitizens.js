Ext.define('B4.store.politicauth.ReceptionCitizens', {
    extend: 'B4.base.Store',
    requires: ['B4.model.politicauth.WorkMode'],
    autoLoad: false,
    storeId: 'politicAuthorityReceptionCitizens',
    model: 'B4.model.politicauth.WorkMode'
});