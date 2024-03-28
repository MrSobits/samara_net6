Ext.define('B4.store.localgov.ReceptionJurPerson', {
    extend: 'B4.base.Store',
    requires: ['B4.model.localgov.WorkMode'],
    autoLoad: false,
    storeId: 'localGovernmentReceptionJurPerson',
    model: 'B4.model.localgov.WorkMode'
});