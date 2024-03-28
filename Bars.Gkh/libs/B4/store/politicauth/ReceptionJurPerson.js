Ext.define('B4.store.politicauth.ReceptionJurPerson', {
    extend: 'B4.base.Store',
    requires: ['B4.model.politicauth.WorkMode'],
    autoLoad: false,
    storeId: 'politicAuthorityReceptionJurPerson',
    model: 'B4.model.politicauth.WorkMode'
});